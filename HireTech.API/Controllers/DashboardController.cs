using HireTech.Core.IServices;
using HireTech.Uitilities.DTO.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HireTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("candidate-statistics/range")]
        public async Task<IActionResult> GetCandidateStatistics(
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate)
        {
            var result=await _dashboardService.GetCandidateStatisticsAsync(fromDate, toDate);
            return CreateResponse(result);
        }
        [HttpGet("quick-stats")]
        public async Task<IActionResult> GetQuickStats()
        {
            var statistics = await _dashboardService.GetCandidateStatisticsAsync();
            return CreateResponse(statistics);
        }
        [HttpGet("CompaniesStatistic")]
        public async Task<IActionResult>GetCompaniesStatistic()
        {
            
            var result=await _dashboardService.GetCompanyStatisticsAsync();
            return CreateResponse(result);
        }
        [HttpGet("VacanciesStatistic")]
        public async Task<IActionResult>GetVacanciesStatistic()
        {
           
            var result = await _dashboardService.GetVacancyStatisticsAsync();
            return CreateResponse(result);
        }
    }
}
