using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Core.Specifications;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HireTech.API.Controllers
{
    [Route("api/applications")]
    [ApiController]
    public class ApplicationsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IUnitOfWork unitOfWork, IMapper mapper, IApplicationService applicationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationService = applicationService;
        }
        [Authorize(Roles = "CANDIDATE")]
        [HttpPost("AddApplication")]
        public async Task<IActionResult> AddApplication([FromBody] CreateApplicationDTO createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var useId = GetUserID();
            if (useId == null)
                return BadRequest($"Candidate with ID {createDto.CandidateId} does not exist");
            var result = await _applicationService.AddApplication(createDto,useId);
            return CreateResponse(result);

            //try
            //{
            //    if (createDto == null)
            //    {
            //        return BadRequest("Application data cannot be null");
            //    }

            //    if (!ModelState.IsValid)
            //    {
            //        return BadRequest(ModelState);
            //    }
            //    var vacancy = await _unitOfWork.Repository<Vacancy>()
            //        .GetByIdAsync(createDto.VacancyId);
            //    if (vacancy == null)
            //    {
            //        return BadRequest($"Vacancy with ID {createDto.VacancyId} does not exist");
            //    }
            //    if (vacancy.Status != "Open")
            //    {
            //        return BadRequest($"Cannot apply to vacancy. Current status: {vacancy.Status}");
            //    }
            //    var useId = GetUserID();
            //    if(useId == null)
            //        return BadRequest($"Candidate with ID {createDto.CandidateId} does not exist");
            //    var spec = new ProfileWithSpecification(useId);
            //    var candidatProfile =await _unitOfWork.Repository<CandidateProfile>().GetByIdWithSpecAsync(spec);
            //    var existingApplication = await CheckExistingApplication(candidatProfile.Id, createDto.VacancyId);
            //    if (existingApplication)
            //    {
            //        return Conflict("Candidate has already applied to this vacancy");
            //    }
            //    if (!string.IsNullOrEmpty(createDto.Status) && !EnterValidStatus(createDto.Status))
            //    {
            //        return BadRequest("Invalid status. Valid values are: Applied, Under Review, Interviewed, Rejected, Accepted");
            //    }
            //    var application = new Application
            //    {
            //        CandidateId = useId,
            //        CandidateProfileId=candidatProfile.Id,
            //        VacancyId = createDto.VacancyId,
            //        Status = ReturnStatus(0),
            //        AppliedOn = DateTime.UtcNow
            //    };
            //    await _unitOfWork.Repository<Application>().AddAsync(application);
            //    await _unitOfWork.CompleteAsync();
            //    var response = new ApplicationResponseDTO
            //    {
            //        Id = application.Id,
            //        CandidateId = application.CandidateId,
            //        VacancyId = application.VacancyId,
            //        Status = application.Status,
            //        AppliedOn = application.AppliedOn,
            //        VacancyTitle = vacancy.Title,
            //        CompanyName = vacancy.Company?.Name
            //    };
            //    return CreateResponse(new ResponseDTO<object>
            //    {
            //        IsSuccess=true,
            //        Message="Apply Succesful",
            //        Data= response,
            //    });
            //}
            //catch (Exception ex)
            //{
            //    return CreateResponse(new ResponseDTO<object>
            //    {
            //        IsSuccess=false,
            //        Message=$"An Error Accured {ex.Message}",
            //        ErrorCode=ErrorCodes.Excptions
            //    });
            //}
        }
        [Authorize(Roles = "CANDIDATE")]
        [HttpGet("GetAllCandidateApplication")]
        public async Task<IActionResult> GetAllCandidateApplication() 
        {
            var userID = GetUserID();
            if (userID == null)
                return Unauthorized("user not Logged In");
            var result = await _applicationService.GetAllCandidateApplication(userID);
            return CreateResponse(result);
        }
        private async Task<bool> CheckExistingApplication(int candidateProfileId, int vacancyId)
        {
           
            try
            {

                var spec= new ApplicationWithVacancySpecification(candidateProfileId, vacancyId);
                var vacancy = await _unitOfWork.Repository<Application>().GetAllWithSpecAsync(spec);
                if (vacancy.Any())
                    return true;
                //if (vacancy == null)
                //    return false;
                //var result = vacancy.Applications.Where(p => p.CandidateId==candidateId);
                //if (result == null)
                //    return false;
                return false; // Placeholder
            }
            catch
            {
                return false;
            }
        }
    }
}
