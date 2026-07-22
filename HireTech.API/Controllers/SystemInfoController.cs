using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // ليكون متاحاً للجميع للاستعلام عن معلومات النظام
    public class SystemInfoController : ControllerBase
    {
        [HttpGet("info")]
        public IActionResult GetSystemInfo()
        {
            var systemInfo = new
            {
                SystemName = "HireTech API",
                Version = "1.0.0",
                Description = "Recruitment & Applicant Tracking System (ATS)",
                SupportedRoles = new[] { "ADMIN", "RECRUITER", "COMPANY", "CANDIDATE" },
                Modules = new[]
                {
                    new
                    {
                        Controller = "ApplicationsController",
                        Description = "Manages job applications submitted by candidates."
                    },
                    new
                    {
                        Controller = "CompaniesController",
                        Description = "Handles company profiles, registration, and management."
                    },
                    new
                    {
                        Controller = "DashboardController",
                        Description = "Provides analytics and statistics for Admins and Recruiters."
                    },
                    new
                    {
                        Controller = "EventNoteController",
                        Description = "Manages interview events, notes, and scheduling."
                    },
                    new
                    {
                        Controller = "ProfileController",
                        Description = "Handles Candidate and Recruiter profile details."
                    },
                    new
                    {
                        Controller = "RolesController",
                        Description = "Manages system roles and permissions."
                    },
                    new
                    {
                        Controller = "UserController",
                        Description = "Handles authentication, registration, and user accounts."
                    },
                    new
                    {
                        Controller = "VacancyController",
                        Description = "Manages job postings, vacancies, and requirements."
                    }
                }
            };

            return Ok(systemInfo);
        }

        [HttpGet("roles")]
        public IActionResult GetSystemRoles()
        {
            var roles = new[]
            {
                new { Name = "ADMIN", Description = "Full system access and management." },
                new { Name = "RECRUITER", Description = "Posts vacancies, reviews applications, and schedules interviews." },
                new { Name = "COMPANY", Description = "Manages company details and views hired candidates." },
                new { Name = "CANDIDATE", Description = "Applies for job vacancies and tracks application status." }
            };

            return Ok(roles);
        }
    }
}