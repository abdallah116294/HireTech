using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Uitilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Service.UserService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRespository;

        public RoleService(IRoleRepository roleRespository)
        {
            _roleRespository = roleRespository;
        }

        public async Task<ResponseDTO<object>> AddRoleToUserAsync(string email, string roleName)
        {
            try
            {
                var result = await _roleRespository.AddRoleToUserAsync(email, roleName);
                if (result)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Data = null,
                        Message = "Role added to user successfully."
                    };
                }
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "Failed to add role to user.",
                    Data = null,
                    ErrorCode = ErrorCodes.BadRequest
                };
            }
            catch (Exception)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding role to user.",
                    Data = null,
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }
    }
}
