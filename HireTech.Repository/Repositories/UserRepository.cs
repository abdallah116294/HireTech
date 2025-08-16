using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Uitilities.DTO.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            throw new NotImplementedException();
        }

        public  async Task<List<User>> GetAllUsers()
        {
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            return users;
        }

        public async Task<string> GetOtpAsyn(User user)
        {
            return await _userManager.GetAuthenticationTokenAsync(user, "HireTech", "ResetPassword");
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _unitOfWork.Repository<User>().GetAllAsync();
            var specificUser=user.Where(u=>u.Id==id).FirstOrDefault();
            return specificUser;
        }

        public  async Task<bool> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDTO dto, string userRole)
        {
            // Check if role exists, if not create it
            var normalizedRole = userRole.ToUpper();
            if (!await _roleManager.RoleExistsAsync(normalizedRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(normalizedRole));
            }
            var user = new User
            {
                UserName = string.Join(dto.FirstName, dto.LastName),
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                FullName = dto.FirstName + " " + dto.LastName,
                Role= normalizedRole,
                CreatedAt = DateTime.UtcNow,
                
            };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, normalizedRole);
                if (!addToRoleResult.Succeeded)
                {
                    // Log the specific error
                    var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                    Console.WriteLine($"Failed to add user to role '{normalizedRole}': {errors}");

                    await _userManager.DeleteAsync(user);
                    return addToRoleResult;
                }

                // Verify the role was added
                var assignedRoles = await _userManager.GetRolesAsync(user);
                Console.WriteLine($"User assigned roles: {string.Join(", ", assignedRoles)}");
            }
            return result;
        }

        public async Task<bool> RemoveOtpAsync(User user)
        {
            var result = await _userManager.RemoveAuthenticationTokenAsync(user, "HireTech", "ResetPassword");
            return result.Succeeded;

        }

        public  async Task<IdentityResult> ResetPasswordAsync(User user, string resetToken, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        }

        public async Task<bool> SetOtpAync(User user, string otpCode)
        {
            var result = await _userManager.SetAuthenticationTokenAsync(user, "HireTech", "ResetPassword", otpCode);
            return result.Succeeded;
        }
    }
}
