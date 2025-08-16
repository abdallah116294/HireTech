using HireTech.Core.Entities;
using HireTech.Uitilities.DTO.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IRepositories
{
    public interface IUserRepository
    {
        Task LogoutAsync();
        Task<bool> LoginAsync(LoginDTO dto);
        Task<IdentityResult> RegisterAsync(RegisterDTO dto,string userRole);
        Task<User> FindByEmailAsync(string email);
        Task<bool> SetOtpAync(User user, string otpCode);
        Task<string> GetOtpAsyn(User user);
        Task<bool> RemoveOtpAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string resetToken, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(string id);
    }
}
