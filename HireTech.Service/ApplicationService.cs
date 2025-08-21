using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Core.Specifications;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace HireTech.Service
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<object>> AddApplication(CreateApplicationDTO createDto, string userId)
        {
            try
            {
                if (createDto == null)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess=false,
                        Message= "Application data cannot be null",
                        ErrorCode=ErrorCodes.BadRequest
                    };

                }
                var vacancy = await _unitOfWork.Repository<Vacancy>()
                    .GetByIdAsync(createDto.VacancyId);
                if (vacancy == null)
                {
                    return new ResponseDTO<object> { IsSuccess = false, Message = $"Vacancy with ID {createDto.VacancyId} does not exist", ErrorCode = ErrorCodes.BadRequest };
                }
                if (vacancy.Status != "Open")
                {
                    return new ResponseDTO<object> { IsSuccess = false, Message = $"Cannot apply to vacancy. Current status: {vacancy.Status}", ErrorCode = ErrorCodes.BadRequest };
                }
                var spec = new ProfileWithSpecification(userId);
                var candidatProfile = await _unitOfWork.Repository<CandidateProfile>().GetByIdWithSpecAsync(spec);
                var existingApplication = await CheckExistingApplication(candidatProfile.Id, createDto.VacancyId);
                if (existingApplication)
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Candidate has already applied to this vacancy"
                    };
                  
                }
                if (!string.IsNullOrEmpty(createDto.Status) && !EnterValidStatus(createDto.Status))
                {
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Invalid status. Valid values are: Applied, Under Review, Interviewed, Rejected, Accepted"
                    };
                   
                }
                var application = new Application
                {
                    CandidateId = userId,
                    CandidateProfileId = candidatProfile.Id,
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
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Apply Succesful",
                    Data = response,
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
        private string ReturnStatus(int index)
        {
            var validStatuses = new[] { "Applied", "Under Review", "Interviewed", "Rejected", "Accepted" };
            // if (index == validStatuses[index])
            if (index <= validStatuses.Length)
                return validStatuses[index];
            return null;
        }
        private bool EnterValidStatus(string status)
        {
            var validStatuses = new[] { "Applied", "Under Review", "Interviewed", "Rejected", "Accepted" };
            return validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
        }
        private async Task<bool> CheckExistingApplication(int candidateProfileId, int vacancyId)
        {

            try
            {

                var spec = new ApplicationWithVacancySpecification(candidateProfileId, vacancyId);
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

        public async Task<ResponseDTO<object>> GetAllCandidateApplication(string usrId)
        {
            try
            {
                var applicationRepo = _unitOfWork.Repository<Application>();
                var spec = new ApplicationWithVacancySpecification(usrId);
                var CandidateApplications = await applicationRepo.GetAllWithSpecAsync(spec);
                if (CandidateApplications == null)
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Candidate Applications Empty",
                        ErrorCode = ErrorCodes.NotFound,
                    };
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
                    SalaryRange = c.Vacancy.SalaryMin.HasValue && c.Vacancy.SalaryMax.HasValue ? $"{c.Vacancy.SalaryMin.Value} -{c.Vacancy.SalaryMax.Value}" : null,

                }).ToList();
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Get All Candidate Applications ",
                    Data = candidateApplications

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
    }
}
