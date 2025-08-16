using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Entities
{
    public class Vacancy:BaseEntity
    {
        // Job details
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Status { get; set; } = "Open"; // Open, Closed, On Hold
        // Salary (optional)
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        // Relations
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        // Recruiter who created it
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //Navigation
        public ICollection<Application> Applications { get; set; }
        public ICollection<EventNote> Events { get; set; }
    }
}
