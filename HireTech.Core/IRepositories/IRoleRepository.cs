using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IRepositories
{
    public interface IRoleRepository
    {
        Task<bool> AddRoleToUserAsync(string email, string roleName);
    }
}
