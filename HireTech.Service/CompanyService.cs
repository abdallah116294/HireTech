using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Core.Specifications;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<object>> CreateCompany(CreateCompanyDTO dto, string userId)
        {
            try
            {
                var companyRepo = _unitOfWork.Repository<Company>();
                var mappedCompany = _mapper.Map<Company>(dto);
                mappedCompany.CreatedAt = DateTime.UtcNow;

                var company = new Company
                {
                    Name = dto.Name,
                    Industry = dto.Industry,
                    Website = dto.Website,
                    Description = dto.Description,
                    CreatedById = userId,
                    CreatedAt = DateTime.UtcNow,
                };
                await companyRepo.AddAsync(company);
                await _unitOfWork.CompleteAsync();
                return new ResponseDTO <object>
                {
                    IsSuccess = true,
                    Message = "Add Company Succesful",
                    Data = company,
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

        public async Task<ResponseDTO<object>> DeleteCompany(int id)
        {
            try
            {
                var companyRepo = _unitOfWork.Repository<Company>();
                if (id == null)
                    return new ResponseDTO<object> { IsSuccess = false, Message = "Id is Null", ErrorCode = ErrorCodes.BadRequest };

                await companyRepo.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
                return new ResponseDTO<object> { IsSuccess = true, Message = "Delete Company Succesful", };
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

        public async Task<ResponseDTO<object>> GetCompanyDetailsById(int id)
        {
            try
            {
                var spec = new CompanyWithSpecification(id);
                var companyRepo = _unitOfWork.Repository<Company>();
                if (id == null)
                    return new ResponseDTO<object> { IsSuccess = false, Message = "Id is Null", ErrorCode = ErrorCodes.BadRequest };
                var companyById = await companyRepo.GetByIdWithSpecAsync(spec);
                Console.WriteLine($"Company By ID {companyById}");
                if (companyById == null)
                    return new ResponseDTO<object> { IsSuccess = false, Message = "No Company Found", ErrorCode = ErrorCodes.NotFound };
                var dto = new CompanyDetailsDTO 
                {
                    Id=companyById.Id,
                    Name=companyById.Name,
                    CreateById=companyById.CreatedById,
                    Description=companyById.Description,
                    Website=companyById.Website,
                    Industry= companyById.Industry,
                    Vacancies=companyById.Vacancies.Select(v=> new VacancyDto 
                    {
                        Id=v.Id,
                        Title=v.Title,
                        Description =v.Description,
                        Requirements=v.Requirements,
                        Status = v.Status,
                    }).ToList()
                };
                return new ResponseDTO<object> { IsSuccess = true, Message = "Get Company Details Succesful", Data = dto };
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

        public async Task<ResponseDTO<object>> SearchByName(string Name)
        {
            try
            {
                var spec = new CompanyWithSpecification();
                var companyRepo = _unitOfWork.Repository<Company>();
                var companies = await companyRepo.GetAllWithSpecAsync(spec);
                var searchedCompany = companies.Where(c => c.Name.Contains(Name, StringComparison.OrdinalIgnoreCase));
                if (searchedCompany == null)
                    return new ResponseDTO<object> { IsSuccess = true, Message = "No Company Found", ErrorCode = ErrorCodes.NotFound };
                return new ResponseDTO<object> { IsSuccess = true, Message = "Get Company", Data = searchedCompany };
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

        public async Task<ResponseDTO<object>> UpdateCompanyData(int id, UpdateCompanyDTO company)
        {
            try
            {
                var companyRepo = _unitOfWork.Repository<Company>();
                await companyRepo.UpdateAsync(id, entity =>
                {
                    if (!string.IsNullOrEmpty(company.Name)) entity.Name = company.Name;
                    if (!string.IsNullOrEmpty(company.Industry)) entity.Industry = company.Industry;
                    if (!string.IsNullOrEmpty(company.Description)) entity.Description = company.Description;
                    if (!string.IsNullOrEmpty(company.Website)) entity.Website = company.Website;
                    entity.CreatedAt = DateTime.Now;
                });
                await _unitOfWork.CompleteAsync();
                var ComapanyAfterUpdate = await companyRepo.GetByIdAsync(id);
                return new ResponseDTO<object> { IsSuccess = true, Message = "Update Company Succesful", Data = ComapanyAfterUpdate };
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
