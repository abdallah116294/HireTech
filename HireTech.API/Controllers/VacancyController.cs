using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Core.Specifications;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Vacancy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HireTech.API.Controllers
{
    [Route("api/Vacancy")]
    [ApiController]
    public class VacancyController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVacancyService _vacancyService;

        public VacancyController(IUnitOfWork unitOfWork, IMapper mapper, IVacancyService vacancyService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _vacancyService = vacancyService;
        }
        [Authorize(Roles = "RECRUITER")]
        [HttpPost("CreateVacancy")]
        public async Task<IActionResult> CreateVacancy([FromBody] CreateVacancyDTO dto)
        {
            var userId = User.FindFirstValue("id");
            if (userId == null)
                return Unauthorized("User Not Login ");
            if (IsValidStatus(dto.Status) == false)
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = "Enter Valid Status",
                    ErrorCode = ErrorCodes.NotFound,
                });
            var result = await _vacancyService.CreateVacancy(dto, userId);
            return CreateResponse(result);
        }
        [HttpGet("GetVacancyById{id}")]
        public async Task<IActionResult>GetVacancyById(int id)
        {
            var result = await _vacancyService.GetVacancyById(id);
            return CreateResponse(result);
        }
        [HttpGet("GetAllVacancies")]
        public async Task<IActionResult> GetAllVacancies()
        {
            var result = await _vacancyService.GetAllVacancies();
            return CreateResponse(result);
        }
        [Authorize(Roles = "RECRUITER")]
        [HttpPut("UpdateVacancy")]
        public async Task<IActionResult> UpdateVacancy([FromQuery] int id,[FromForm]UpdateVacancyDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Update data cannot be null");
            }
            if (!string.IsNullOrEmpty(dto.Status) &&
           !IsValidStatus(dto.Status))
            {
                return BadRequest("Invalid status. Valid values are: Open, Closed, On Hold");
            }
            var userID = User.FindFirstValue("id");
            var result = await _vacancyService.UpdateVacancy(id,dto,userID);
            return CreateResponse(result);
        }
        [HttpGet("GetCandidatesApplication{id}")]
        public async Task<IActionResult> GetCandidatesApplication (int id)
        {
            var result = await _vacancyService.GetCandidatesApplication(id);
            return CreateResponse(result);
        }

        [HttpPut("AddStatustCandidate")]
        public async Task<IActionResult> AcceptCandidate(string Status,int vacancyId,string candidateId, int ApplicationId)
        {
            if (EnterValidStatus(Status) == false)
                return BadRequest("Enter Valid Status");
            var result = await _vacancyService.AccecptCandidate(Status, candidateId, vacancyId, ApplicationId);
            return CreateResponse(result);
        }
    }
}
