using HireTech.Uitilities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            return User.FindFirstValue("id");
        }
        protected  bool IsValidStatus(string status)
        {
            var validStatuses = new[] { "Open", "Closed", "On Hold" };
            return validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
        }
        protected bool EnterValidStatus(string status)
        {
            var validStatuses = new[] { "Applied", "Under Review", "Interviewed", "Rejected", "Accepted" };
            return validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
        }
        protected string ReturnStatus(int index)
        {
            var validStatuses = new[] { "Applied", "Under Review", "Interviewed", "Rejected", "Accepted" };
            // if (index == validStatuses[index])
            if (index <= validStatuses.Length)
                return validStatuses[index];
            return null;
        }
    }
}
