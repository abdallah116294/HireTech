using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Vacancy
{
    public class UpdateVacancyDTO
    {

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public string? Status { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public int? CompanyId { get; set; }
    }
}
