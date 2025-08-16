using HireTech.Core.Entities;
using HireTech.Core.IServices;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.User;
using HireTech.Uitilities.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HireTech.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> userManager;
        private readonly TokenHelper _tokenHelper;

        public UserController(IUserService userService, UserManager<User> userManager, TokenHelper tokenHelper)
        {
            _userService = userService;
            this.userManager = userManager;
            _tokenHelper = tokenHelper;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO dto)
        {
            var response = await _userService.LoginAsync(dto);
            if (response.IsSuccess && response.Data != null)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Data = response,
                    Message = "Login successful."
                });
            }
            else
            {
                return CreateResponse(response);
            }
        }
        [HttpPost("defual-register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO dto)
        {
            var response = await _userService.RegisterAsync(dto, "Candidate");
            return CreateResponse(response);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _userService.LogoutAsync();
            return Ok(new ResponseDTO<object> { IsSuccess = true, Message = "Logged out successfully." });
        }
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDTO dto)
        {
            var response = await _userService.ForgetPassword(dto);
            return CreateResponse(response);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var response = await _userService.ResetPasswordAsync(dto.Email, dto.otp, dto.NewPassword);
            return CreateResponse(response);
        }
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            var seller = await _userService.GetAllUser();
            return CreateResponse(seller);
        }
    }
}
