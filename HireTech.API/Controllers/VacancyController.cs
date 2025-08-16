using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
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
    }
}
