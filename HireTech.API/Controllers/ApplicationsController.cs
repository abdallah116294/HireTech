using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
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

        public ApplicationsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [Authorize(Roles = "CANDIDATE")]
        [HttpPost("AddApplication")]
        public async Task<IActionResult> AddApplication([FromBody] CreateApplicationDTO createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("Application data cannot be null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var vacancy = await _unitOfWork.Repository<Vacancy>()
                    .GetByIdAsync(createDto.VacancyId);
                if (vacancy == null)
                {
                    return BadRequest($"Vacancy with ID {createDto.VacancyId} does not exist");
                }
                if (vacancy.Status != "Open")
                {
                    return BadRequest($"Cannot apply to vacancy. Current status: {vacancy.Status}");
                }
                var useId = GetUserID();
                if(useId == null)
                    return BadRequest($"Candidate with ID {createDto.CandidateId} does not exist");
                var spec = new ProfileWithSpecification(useId);
                var candidatProfile =await _unitOfWork.Repository<CandidateProfile>().GetByIdWithSpecAsync(spec);
                var existingApplication = await CheckExistingApplication(candidatProfile.Id, createDto.VacancyId);
                if (existingApplication)
                {
                    return Conflict("Candidate has already applied to this vacancy");
                }
                if (!string.IsNullOrEmpty(createDto.Status) && !EnterValidStatus(createDto.Status))
                {
                    return BadRequest("Invalid status. Valid values are: Applied, Under Review, Interviewed, Rejected, Accepted");
                }
                var application = new Application
                {
                    CandidateId = useId,
                    CandidateProfileId=candidatProfile.Id,
                    VacancyId = createDto.VacancyId,
                    Status = ReturnStatus(0),
                    AppliedOn = DateTime.UtcNow
                };
                await _unitOfWork.Repository<Application>().AddAsync(application);
                await _unitOfWork.CompleteAsync();
                var response = new ApplicationResponseDTO
                {
                    Id = application.Id,
                    CandidateId = application.CandidateId,
                    VacancyId = application.VacancyId,
                    Status = application.Status,
                    AppliedOn = application.AppliedOn,
                    VacancyTitle = vacancy.Title,
                    CompanyName = vacancy.Company?.Name
                };
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess=true,
                    Message="Apply Succesful",
                    Data= response,
                });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess=false,
                    Message=$"An Error Accured {ex.Message}",
                    ErrorCode=ErrorCodes.Excptions
                });
            }
        }
        [Authorize(Roles = "CANDIDATE")]
        [HttpGet("GetAllCandidateApplication")]
        public async Task<IActionResult> GetAllCandidateApplication() 
        {
            try
            {
                var userID = GetUserID();
                if (userID == null)
                    return Unauthorized("user not Logged In");
                var applicationRepo = _unitOfWork.Repository<Application>();
                var spec = new ApplicationWithVacancySpecification(userID);
                var CandidateApplications = await applicationRepo.GetAllWithSpecAsync(spec);
                if (CandidateApplications == null)
                    return CreateResponse( new ResponseDTO<object>
                    {
                        IsSuccess=false,
                        Message= "Candidate Applications Empty",
                        ErrorCode=ErrorCodes.NotFound,
                    });
                List<CandidateApplicationDTO> candidateApplications = CandidateApplications.Select(c => new CandidateApplicationDTO
                {
                    ApplicationId = c.Id,
                    Status = c.Status,
                    VacancyId = c.VacancyId,
                    VacancyTitle = c.Vacancy.Title,
                    VacancyDescription = c.Vacancy.Description,
                    VacancyStatus = c.Vacancy.Status,
                    SalaryMin = c.Vacancy.SalaryMin,
                    SalaryMax = c.Vacancy.SalaryMax,
                    CompanyId = c.Vacancy.CompanyId,
                    CompanyName = c.Vacancy.Company.Name,
                    DaysSinceApplication = (DateTime.Now - c.AppliedOn).Days,
                    SalaryRange = c.Vacancy.SalaryMin.HasValue && c.Vacancy.SalaryMax.HasValue ? $"{c.Vacancy.SalaryMin.Value} -{ c.Vacancy.SalaryMax.Value}" :null,

                }).ToList();
                return CreateResponse(new ResponseDTO<IEnumerable<object>>
                {
                    IsSuccess = true,
                    Message = "Get All Candidate Applications ",
                    Data = candidateApplications

                });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                });
            }
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
