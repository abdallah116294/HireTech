using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Core.Specifications;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Vacancy;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Service
{
    public class VacancyService : IVacancyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VacancyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<object>> CreateVacancy(CreateVacancyDTO dto, string userId)
        {
            try
            {
                if (dto == null)
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "An value null"
                    };

                var vacancyRepo = _unitOfWork.Repository<Vacancy>();
                var vacancy = new Vacancy
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    CompanyId = dto.CompanyId,
                    Requirements = dto.Requirements,
                    SalaryMax = dto.SalaryMax,
                    SalaryMin = dto.SalaryMin,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = userId,
                    Status = dto.Status,

                };

                await vacancyRepo.AddAsync(vacancy);
                await _unitOfWork.CompleteAsync();
                return new ResponseDTO<object> 
                {
                    IsSuccess = true,
                    Message = "Creat Vacancy Succesfull",
                    Data = vacancy,
                };
            }
            catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }

        public async Task<ResponseDTO<object>> GetAllVacancies()
        {
            try
            {
                var spec = new VacancyWithCompanyDetailsSpecification();
                var vacancyRepo = _unitOfWork.Repository<Vacancy>();
                var Vacancies = await vacancyRepo.GetAllWithSpecAsync(spec);
                if (Vacancies == null)
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "no vacancy found",
                        ErrorCode = ErrorCodes.NotFound,
                    };
                var vacanciesMapped = _mapper.Map<IEnumerable<VacancyResponseDTO>>(Vacancies);
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Get Vacancy Succefull ",
                    Data = vacanciesMapped,
                };
            }
            catch(Exception  ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }

        public async Task<ResponseDTO<object>> GetCandidatesApplication(int id)
        {
            try
            {
                if (id == null)
                    new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Id must not be null",
                        ErrorCode = ErrorCodes.BadRequest,
                    };
                var spec = new ApplicationWithVacancySpecification(id);
                var vacancyRepo = _unitOfWork.Repository<Application>();
                var Applications = await vacancyRepo.GetAllWithSpecAsync(spec);
                if (Applications == null || !Applications.Any())
                    return new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Message = "no Applications on this Vacancy yet ",
                    };
                var dtoList = Applications.Select(a => new ApplicationsOnVancancyDTO
                {
                    ApplicationId = a.Id,
                    Status = a.Status,
                    AppliedOn = a.AppliedOn,

                    CandidateId = a.CandidateId,
                    CandidateName = a.Candidate.FullName,
                    CandidateEmail = a.Candidate.Email,

                    VacancyId = a.VacancyId,
                    VacancyTitle = a.Vacancy.Title,
                    VacancyDescription = a.Vacancy.Description,

                    CompanyId = a.Vacancy.CompanyId,
                    CompanyName = a.Vacancy.Company.Name,

                    DaysSinceApplication = (DateTime.Now - a.AppliedOn).Days,
                }).ToList();
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Get All Candidate that Applay on Vacancy",
                    Data = dtoList,
                };
            }  catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }

        public async Task<ResponseDTO<object>> GetVacancyById(int id)
        {
            try
            {
                var spec = new VacancyWithCompanyDetailsSpecification(id);
                var vacancyRepo = _unitOfWork.Repository<Vacancy>();
                var vacancy = await vacancyRepo.GetByIdWithSpecAsync(spec);
                if (vacancy == null)
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "no vacancy with this ID",
                        ErrorCode = ErrorCodes.NotFound,
                    };
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
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Get Vacancy Succefull ",
                    Data = vacancyDTO,
                };
            }
            catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }

        public  async Task<ResponseDTO<object>> UpdateVacancy(int id, UpdateVacancyDTO dto, string userId)
        {
            try
            {
                if (dto.SalaryMin.HasValue && dto.SalaryMax.HasValue &&
               dto.SalaryMin > dto.SalaryMax)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Minimum salary cannot be greater than maximum salary",
                        ErrorCode=ErrorCodes.BadRequest
                    };
                  
                }
                if (dto.CompanyId.HasValue)
                {
                    var companyExists = await _unitOfWork.Repository<Company>()
                        .GetByIdAsync(dto.CompanyId.Value);
                    if (companyExists == null)
                    {
                        return new ResponseDTO<object>
                        {
                            IsSuccess = false,
                            Message = $"Company with ID {dto.CompanyId} does not exist",
                            ErrorCode = ErrorCodes.BadRequest
                        };
                      
                    }
                }
                var vacancyRepo = _unitOfWork.Repository<Vacancy>();
                var spec = new VacancyWithCompanyDetailsSpecification(id);
                var vacancy = await vacancyRepo.GetByIdWithSpecAsync(spec);
                var mappedVacancy = _mapper.Map<VacancyResponseDTO>(vacancy);
                if (mappedVacancy.CreatedBy.Id == userId)
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
                    return new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Message = "Updat Vacancy Succesful",
                        Data = mappedVacancyAfterupdate,
                    };
                }
                else
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "User Not Allowed",
                        ErrorCode = ErrorCodes.UnAuthorized,
                    };
                    //  return Unauthorized("User Not Allowed ");
                }
            }
            catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }
    }
}
