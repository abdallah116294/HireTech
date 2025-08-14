using HireTech.Uitilities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult CreateResponse<T>(ResponseDTO<T> response) where T : class 
        {
            if (response.IsSuccess)
                return Ok(response);
            return response.ErrorCode switch
            {
                ErrorCodes.NotFound => NotFound(response),
                ErrorCodes.BadRequest=>BadRequest(response),
                ErrorCodes.UnAuthorized=>Unauthorized(response),
                ErrorCodes.Excptions=>StatusCode(StatusCodes.Status500InternalServerError,response),
            };
        }
        protected string GetUserID()
        {
            return User.Claims.FirstOrDefault(x=>x.Type=="role")?.Value??string.Empty;
        }
    }
}
