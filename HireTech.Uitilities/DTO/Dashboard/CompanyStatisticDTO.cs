using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Dashboard
{
    public class CompanyStatisticsDto
    {
        public int TotalCompanies { get; set; }
        public int ActiveCompanies { get; set; }
        public List<IndustryCountDto> CompaniesByIndustry { get; set; }
        //public List<MonthlyCompanyDto> CompaniesCreatedByMonth { get; set; }
        //public List<TopCompanyDto> TopCompaniesByVacancies { get; set; }
        //public List<TopCompanyDto> TopCompaniesByApplications { get; set; }
    }
}
