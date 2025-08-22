using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Dashboard
{
    public class DashboardStatisticsDto
    {
        public CandidateStatisticsDto Applications { get; set; }
        public CompanyStatisticsDto Companies { get; set; }
        public VacancyStatisticsDto Vacancies { get; set; }
        public OverviewStatisticsDto Overview { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
    public class TopVacancyApplicationDto
    {
        public int VacancyId { get; set; }
        public string VacancyTitle { get; set; }
        public string CompanyName { get; set; }
        public int ApplicationCount { get; set; }
    }

    public class IndustryCountDto
    {
        public string Industry { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class MonthlyCompanyDto
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
    }

    public class TopCompanyDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public int Count { get; set; }
        public string Website { get; set; }
    }

    public class VacancyStatusCountDto
    {
        public string Status { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class MonthlyVacancyDto
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
    }

    public class SalaryRangeDto
    {
        public string Range { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }
    public class OverviewStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int TotalRecruiters { get; set; }
        public double ApplicationSuccessRate { get; set; }
        public double VacancyFillRate { get; set; }
        public string MostActiveIndustry { get; set; }
        public string MostDemandedSkill { get; set; }
    }


}
