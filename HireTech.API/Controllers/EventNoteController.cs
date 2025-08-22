using HireTech.Core.IServices;
using HireTech.Uitilities.DTO.EventNote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace HireTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventNoteController : BaseController
    {
        private readonly IEventNoteService _eventNoteService;

        public EventNoteController(IEventNoteService eventNoteService)
        {
            _eventNoteService = eventNoteService;
        }
        [Authorize(Roles = "RECRUITER")]
        [HttpPost("CreateEventNote")]
        public async Task<IActionResult>CreateEventNote(EventNoteRequestDTO dto)
        {
            var userId = GetUserID();
            if (userId == null) 
                return Unauthorized("Pleas Login to Create Event");
            var result = await _eventNoteService.CreateEventNote(dto, userId);
            return CreateResponse(result);
        }
        [HttpGet("GetAlEventNote")]
        public async Task<IActionResult> GetAllEventNote()
        {
            var result = await _eventNoteService.GetAllEventNote();
            return CreateResponse(result);
        }
    }
}
