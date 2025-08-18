using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Vacancy
{
    // Response DTOs
    public class VacancyResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Status { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public DateTime CreatedAt { get; set; }

        // Company info without circular reference
        public CompanyBasicInfoDTO Company { get; set; }

        // Creator info without circular reference
        public UserBasicInfoDTO CreatedBy { get; set; }
    }

    public class CompanyBasicInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
    }

    public class UserBasicInfoDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
