using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Entities
{
    public class CandidateProfile:BaseEntity
    {
        //Forgen Key for user 
        public string UserId { get; set; }
        public User User { get; set; }
        //Personal Info
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        //Education & Expreience
        public string HighestQualification { get; set; }
        public string University { get; set; }
        public int? GraduationYear { get; set; }
        public int YearsOfExperience { get; set; }
        //Career Info
        public string CurrentJobTitle { get; set; }
        public string CurrentCompany { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? ExpectedSalary { get; set; }
        //Contact Info
        public string LinkedInProfile { get; set; }
        public string PortfolioUrl { get; set; }
        // Navigation Properties
        public ICollection<Application> Applications { get; set; }
        public ICollection<Skill> Skills { get; set; }
    }
}
