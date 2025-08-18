using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.Specifications;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Vacancy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HireTech.API.Controllers
{
    [Route("api/Vacancy")]
    [ApiController]
    public class VacancyController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VacancyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [Authorize(Roles = "RECRUITER")]
        [HttpPost("CreateVacancy")]
        public async Task<IActionResult> CreateVacancy([FromBody] CreateVacancyDTO dto)
        {
            try
            {
                var userId = User.FindFirstValue("id");
                if(userId==null)
                    return Unauthorized("User Not Login ");
                var vacancyRepo = _unitOfWork.Repository<Vacancy>();
                var mappedVacancy=_mapper.Map<Vacancy>(dto);
                var vacancy = new Vacancy { 
                    Title = dto.Title,
                    Description=dto.Description,
                    CompanyId=dto.CompanyId,
                    Requirements = dto.Requirements,
                    SalaryMax=dto.SalaryMax,
                    SalaryMin=dto.SalaryMin,
                    CreatedAt=DateTime.UtcNow,
                    CreatedById=userId,
                    Status=dto.Status,
                };

                await vacancyRepo.AddAsync(vacancy);
               await _unitOfWork.CompleteAsync();

                return CreateResponse(new ResponseDTO<object> 
                {
                    IsSuccess = true,
                    Message="Creat Vacancy Succesfull",
                    Data=vacancy,

                });

            }
            catch (Exception ex) 
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                   ErrorCode=ErrorCodes.Excptions
                });
            }
        }
        [HttpGet("GetVacancyById{id}")]
        public async Task<IActionResult>GetVacancyById(int id)
        {
            try
            {
                var spec = new VacancyWithCompanyDetailsSpecification(id);
                var vacancyRepo = _unitOfWork.Repository<Vacancy>();
                var vacancy =await vacancyRepo.GetByIdWithSpecAsync(spec);
                if (vacancy == null)
                    return CreateResponse(new ResponseDTO<object> 
                    {
                        IsSuccess=false,
                        Message="no vacancy with this ID",
                        ErrorCode=ErrorCodes.NotFound,
                    });
                var vacancyDTO = new VacancyResponseDTO 
                {
                    Id = vacancy.Id,
                    Title = vacancy.Title,
                    Description = vacancy.Description,
                    Requirements = vacancy.Requirements,
                    Status = vacancy.Status,
                    SalaryMin = vacancy.SalaryMin,
                    SalaryMax = vacancy.SalaryMax,
                    CreatedAt = vacancy.CreatedAt,
                    Company = new CompanyBasicInfoDTO
                    {
                        Id = vacancy.Company.Id,
                        Name = vacancy.Company.Name,
                        Industry = vacancy.Company.Industry,
                        Website = vacancy.Company.Website,
                        Description = vacancy.Company.Description
                    },
                    CreatedBy = new UserBasicInfoDTO
                    {
                        Id = vacancy.CreatedBy.Id,
                        FullName = vacancy.CreatedBy.FullName,
                        Email = vacancy.CreatedBy.Email,
                        Role = vacancy.CreatedBy.Role
                    }
                };
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Get Vacancy Succefull ",
                    Data= vacancyDTO,
                });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions,
                });
            }
        }
        [HttpGet("GetAllVacancies")]
        public async Task<IActionResult> GetAllVacancies()
        {
            try
            {
                var spec = new VacancyWithCompanyDetailsSpecification();
                var vacancyRepo = _unitOfWork.Repository<Vacancy>();
                var Vacancies = await vacancyRepo.GetAllWithSpecAsync(spec);
                if(Vacancies==null)
                    return CreateResponse(new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "no vacancy found",
                        ErrorCode = ErrorCodes.NotFound,
                    });
                var vacanciesMapped = _mapper.Map<IEnumerable<VacancyResponseDTO>>(Vacancies);
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Get Vacancy Succefull ",
                    Data = vacanciesMapped,
                });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions,
                });
            }
        }
        [Authorize(Roles = "RECRUITER")]
        [HttpPut("UpdateVacancy")]
        public async Task<IActionResult> UpdateVacancy([FromQuery] int id,[FromForm]UpdateVacancyDTO dto)
        {
            try
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
                if (dto.SalaryMin.HasValue && dto.SalaryMax.HasValue &&
                       dto.SalaryMin > dto.SalaryMax)
                {
                    return BadRequest("Minimum salary cannot be greater than maximum salary");
                }
                if (dto.CompanyId.HasValue)
                {
                    var companyExists = await _unitOfWork.Repository<Company>()
                        .GetByIdAsync(dto.CompanyId.Value);
                    if (companyExists == null)
                    {
                        return BadRequest($"Company with ID {dto.CompanyId} does not exist");
                    }
                }
                var userID = User.FindFirstValue("id");
                
                var vacancyRepo = _unitOfWork.Repository<Vacancy>();
                var spec = new VacancyWithCompanyDetailsSpecification(id);
                var vacancy = await vacancyRepo.GetByIdWithSpecAsync(spec);
                var mappedVacancy = _mapper.Map<VacancyResponseDTO>(vacancy);
                if(mappedVacancy.CreatedBy.Id==userID)
                {
                    await _unitOfWork.Repository<Vacancy>().UpdateAsync(id, vacancy =>
                    {
                        // Only update fields that are provided (not null)
                        if (!string.IsNullOrWhiteSpace(dto.Title))
                            vacancy.Title = dto.Title;

                        if (!string.IsNullOrWhiteSpace(dto.Description))
                            vacancy.Description = dto.Description;

                        if (!string.IsNullOrWhiteSpace(dto.Requirements))
                            vacancy.Requirements = dto.Requirements;

                        if (!string.IsNullOrWhiteSpace(dto.Status))
                            vacancy.Status = dto.Status;

                        if (dto.SalaryMin.HasValue)
                            vacancy.SalaryMin = dto.SalaryMin;

                        if (dto.SalaryMax.HasValue)
                            vacancy.SalaryMax = dto.SalaryMax;

                        if (dto.CompanyId.HasValue)
                            vacancy.CompanyId = dto.CompanyId.Value;
                    });
                    await _unitOfWork.CompleteAsync();
                    var vacancyAfterupdate = await vacancyRepo.GetByIdWithSpecAsync(spec);
                    var mappedVacancyAfterupdate = _mapper.Map<VacancyResponseDTO>(vacancy);
                    return CreateResponse(new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Message = "Updat Vacancy Succesful",
                        Data = mappedVacancyAfterupdate,
                    });
                }
                else
                {
                    return Unauthorized("User Not Allowed ");
                }
               
            }catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"An Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions,
                });
            }
        }
        private static bool IsValidStatus(string status)
        {
            var validStatuses = new[] { "Open", "Closed", "On Hold" };
            return validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
        }
    }
}
