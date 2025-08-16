using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Vacancy
{
    public class CreateVacancyDTO
    {
        [Required(ErrorMessage = "Job title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Job description is required")]
        [StringLength(5000, ErrorMessage = "Description cannot exceed 5000 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Job requirements are required")]
        [StringLength(3000, ErrorMessage = "Requirements cannot exceed 3000 characters")]
        public string Requirements { get; set; }
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; } = "Open";
        [Range(0, double.MaxValue, ErrorMessage = "Minimum salary must be a positive number")]
        public decimal? SalaryMin { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maximum salary must be a positive number")]
        public decimal? SalaryMax { get; set; }

        [Required(ErrorMessage = "Company ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid company")]
        public int CompanyId { get; set; }
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (SalaryMin.HasValue && SalaryMax.HasValue && SalaryMin > SalaryMax)
            {
                errorMessage = "Minimum salary cannot be greater than maximum salary";
                return false;
            }

            return true;
        }
    }
}
