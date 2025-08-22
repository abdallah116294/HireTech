using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Dashboard
{
    public class VacancyStatisticsDto
    {
        public int TotalVacancies { get; set; }
        public int OpenVacancies { get; set; }
        public int ClosedVacancies { get; set; }
        public List<VacancyStatusCountDto> VacanciesByStatus { get; set; }
        public List<MonthlyVacancyDto> VacanciesCreatedByMonth { get; set; }
        public List<SalaryRangeDto> VacanciesBySalaryRange { get; set; }
        public double AverageApplicationsPerVacancy { get; set; }
    }
}
