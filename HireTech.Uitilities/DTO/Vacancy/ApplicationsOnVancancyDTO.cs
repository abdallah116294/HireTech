using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Vacancy
{
    public class ApplicationsOnVancancyDTO
    {
        public int ApplicationId { get; set; }
        public string Status { get; set; }
        public DateTime AppliedOn { get; set; }

        // Candidate Info
        public string CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }

        // Vacancy Info
        public int VacancyId { get; set; }
        public string VacancyTitle { get; set; }
        public string VacancyDescription { get; set; }

        // Company Info
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        // Extra
        public int DaysSinceApplication { get; set; }
    }
}
