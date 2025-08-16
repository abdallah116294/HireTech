using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HireTech.Core.Entities;
using HireTech.Uitilities.Helpers;

namespace HireTech.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly TokenHelper _tokenHelper;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, UserManager<User> userManager, TokenHelper tokenHelper, IEmailService emailService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _tokenHelper = tokenHelper;
            _emailService = emailService;
        }

        public async Task<ResponseDTO<object>> ForgetPassword(ForgetPasswordDTO dto)
        {
            try
            {
                var user = await _userRepository.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "User not Found",
                        ErrorCode = ErrorCodes.NotFound,

                    };
                }
                var otpCode = new Random().Next(100000, 999999).ToString();
                var result = await _userRepository.SetOtpAync(user, otpCode);
                if (!result)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Erro while Generate Otp"
                    };

                }
                await _emailService.SendEmailAsync(user.Email, "Reset Password Code", $"Your OTP code is: {otpCode}");
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Otp Send to your Email"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "An Error Accured While Foreget Password"
                };
            }
        }

        public async Task<ResponseDTO<object>> GetAllUser()
        {
            try
            {
                var users = await _userRepository.GetAllUsers();
                if (users == null)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "No Seller Found",
                        ErrorCode = ErrorCodes.NotFound,
                    };
                }
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Get All Sellers",
                    Data = users
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "An Error Accured While Foreget Password"
                };
            }
        }

        public async Task<User> GetUserByID(string id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<ResponseDTO<object>> LoginAsync(LoginDTO dto)
        {
            try
            {
                var result = await _userRepository.LoginAsync(dto);
                if (result == true)
                {
                    var user = await _userManager.FindByEmailAsync(dto.Email);

                    // Add null check for user
                    if (user == null)
                    {
                        return new ResponseDTO<object>
                        {
                            IsSuccess = false,
                            Message = "User not found",
                            ErrorCode = ErrorCodes.BadRequest,
                        };
                    }

                    var roles = await _userManager.GetRolesAsync(user);

                    // Ensure we have at least one role
                    var userRole = roles.FirstOrDefault(); // Provide default role if none exists

                    var tokenDto = new TokenDTO
                    {
                        Email = dto.Email, // This should not be null since login succeeded
                        Id = user.Id,      // This should not be null
                        Role = userRole    // This now has a fallback value
                    };

                    var token = _tokenHelper.GenerateToken(tokenDto);

                    return new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Data = new
                        {
                            Email = dto.Email,
                            Token = token,
                            Role = userRole,
                            UserId = user.Id
                        },
                        Message = "Login User Success",
                    };
                }

                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "Invalid credentials",
                    ErrorCode = ErrorCodes.BadRequest,
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An Error Happened While Login user: {ex.Message}", // Use ex.Message instead of full exception
                    ErrorCode = ErrorCodes.Excptions,
                };
            }
        }

        public async Task LogoutAsync()
        {
            await _userRepository.LogoutAsync();
        }

        public async Task<ResponseDTO<object>> RegisterAsync(RegisterDTO dto, string userRole)
        {
            try
            {
                var result = await _userRepository.RegisterAsync(dto,userRole);
                if (!result.Succeeded)
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Error while Register User",
                        ErrorCode = ErrorCodes.BadRequest,
                        //IsSuccess = true,
                        //Message = "Resgiter Successful ",
                        //Data = dto,
                    };
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Resgiter Successful ",
                    Data = dto,
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An  Error Accured while Register User {ex}",
                    ErrorCode = ErrorCodes.Excptions,
                };
            }
        }

        public async Task<ResponseDTO<object>> ResetPasswordAsync(string email, string otp, string newPassword)
        {
            try
            {
                var user = await _userRepository.FindByEmailAsync(email);
                if (user == null)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "No User Found",
                        ErrorCode = ErrorCodes.NotFound,
                    };
                }
                var storedOtp = await _userRepository.GetOtpAsyn(user);
                if (storedOtp == null || storedOtp != otp)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Invalid or expired OTP"
                    };
                }
                var resetToken = await _userRepository.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userRepository.ResetPasswordAsync(user, resetToken, newPassword);
                if (!resetResult.Succeeded)
                    return new ResponseDTO<object>()
                    {
                        IsSuccess = false,
                        Message = "Error while resetting your password"
                    };
                await _userRepository.RemoveOtpAsync(user);
                return new  ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Password has been reset successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "Error Whilre Reset your Password",
                    ErrorCode = ErrorCodes.Excptions,
                };
            }
        }
    }
}
