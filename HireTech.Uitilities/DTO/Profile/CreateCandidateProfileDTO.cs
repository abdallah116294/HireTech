using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Profile
{
    public class CreateCandidateProfileDTO
    {
        // FK from User
        [Required]
        public string UserId { get; set; }

        // Personal Info
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }

        // Education & Experience
        public string HighestQualification { get; set; }
        public string University { get; set; }
        public int? GraduationYear { get; set; }
        public int YearsOfExperience { get; set; }

        // Career Info
        public string CurrentJobTitle { get; set; }
        public string CurrentCompany { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? ExpectedSalary { get; set; }

        // Contact & Portfolio
        public string LinkedInProfile { get; set; }
        public string PortfolioUrl { get; set; }

      
        // Skills (optional at creation)
        public List<string> Skills { get; set; }
    }

}
