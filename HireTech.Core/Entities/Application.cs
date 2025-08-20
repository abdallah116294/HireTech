using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Entities
{
    public class Application:BaseEntity
    {
        public string CandidateId { get; set; }
        public User Candidate { get; set; }
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
        public string Status { get; set; } = "Applied";
        public int CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        public DateTime AppliedOn { get; set; } = DateTime.UtcNow;
    }
}
