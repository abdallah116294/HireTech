using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IServices
{
    public interface IDashboardService
    {
        Task<ResponseDTO<object>> GetCandidateStatisticsAsync();
        Task<ResponseDTO<object>> GetCandidateStatisticsAsync(DateTime fromDate, DateTime toDate);
        //Task<ResponseDTO<object>> GetDashboardStatisticsAsync();
        //Task<ResponseDTO<object>> GetDashboardStatisticsAsync(DateTime fromDate, DateTime toDate);
        Task<ResponseDTO<object>> GetCompanyStatisticsAsync();
        Task<ResponseDTO<object>> GetVacancyStatisticsAsync();
    }
}
