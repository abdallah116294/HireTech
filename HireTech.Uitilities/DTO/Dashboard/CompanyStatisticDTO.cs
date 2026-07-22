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
       
    }
}
