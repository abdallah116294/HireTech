using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Profile
{
    public class CandidateProfileResponseDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
       
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? HighestQualification { get; set; }
        public string? CurrentJobTitle { get; set; }
        public string? CurrentCompany { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? PortfolioUrl { get; set; }
        public List<string> Skills { get; set; } = new();
        public int ApplicationsCount { get; set; }
    }
}
