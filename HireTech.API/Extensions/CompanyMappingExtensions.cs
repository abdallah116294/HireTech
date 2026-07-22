using HireTech.Core.Entities;
using HireTech.Uitilities.DTO.Company;

namespace HireTech.API.Extensions
{
    public static class CompanyMappingExtensions
    {
        //To ResponseDTO
        public static CompanyDetailsDTO toResponse(this Company company)
        {
            return new CompanyDetailsDTO
            {
                Id = company.Id,
                Name = company.Name,
                CreateById = company.CreatedById,
                Description = company.Description,
                Website = company.Website,
                Industry = company.Industry,
                Vacancies = company.Vacancies.Select(v => new VacancyDto
                {
                    Id = v.Id,
                    Title = v.Title,
                    Description = v.Description,
                    Requirements = v.Requirements,
                    Status = v.Status,
                }).ToList()
            };
        }
    }
}
