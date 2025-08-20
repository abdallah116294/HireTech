using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Application
{
    public class CandidateApplicationDTO
    {
        public int ApplicationId { get; set; }
        public string Status { get; set; }
        public DateTime AppliedOn { get; set; }

        public int VacancyId { get; set; }
        public string VacancyTitle { get; set; }
        public string VacancyDescription { get; set; }
        public string VacancyStatus { get; set; }

        public decimal? SalaryMin { get; set; }   // should be nullable
        public decimal? SalaryMax { get; set; }   // should be nullable

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int DaysSinceApplication { get; set; }
        public string SalaryRange { get; set; }
    }
}
