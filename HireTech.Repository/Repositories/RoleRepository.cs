using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Repository.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddRoleToUserAsync(string email, string roleName)
        {
            //Find the user 
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;
            //Check if the role exists
            var existsRole = await _roleManager.RoleExistsAsync(roleName);
            if (!existsRole)
                return false;
            //Add the role to the user
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
                return true;
            return false;
        }
    }
}
