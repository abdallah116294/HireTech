using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Entities
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
        public string  Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //Navigation Properties 
        //Company 
        public ICollection<Company> CompaniesCreated { get; set; }
        //Vancy Created
        public ICollection<Vacancy> VacanciesCreated { get; set; }
        public ICollection<EventNote> EventsCreated { get; set; }
        //Profile
        public CandidateProfile CandidateProfile { get; set; }
    }
}
