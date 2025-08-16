using HireTech.Core.IServices;
using HireTech.Uitilities.DTO.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost]
        public async Task<IActionResult> AddRoleTouser([FromForm] AddRoleToUser dto)
        {
            var result = await _roleService.AddRoleToUserAsync(dto.Email, dto.RoleName);
            return CreateResponse(result);
        }
    }
}
