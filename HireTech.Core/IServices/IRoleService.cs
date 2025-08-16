using HireTech.Uitilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IServices
{
    public interface IRoleService
    {
        Task<ResponseDTO<object>> AddRoleToUserAsync(string email, string roleName);
    }
}
