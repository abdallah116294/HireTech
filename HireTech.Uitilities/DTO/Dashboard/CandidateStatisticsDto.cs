using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Dashboard
{
    public class CandidateStatisticsDto
    {
        public int TotalApplications { get; set; }
        public int TotalCandidates { get; set; }
        public List<ApplicationStatusCountDto> ApplicationsByStatus { get; set; }
        public List<MonthlyApplicationDto> ApplicationsByMonth { get; set; }
        public List<VacancyApplicationDto> TopVacancies { get; set; }
        public double AverageApplicationsPerCandidate { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
