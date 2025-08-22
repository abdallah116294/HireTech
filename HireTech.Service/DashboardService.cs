using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Core.Specifications;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO<object>> GetCandidateStatisticsAsync()
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            return await GetCandidateStatisticsAsync(thirtyDaysAgo, DateTime.UtcNow);
        }

        public async Task<ResponseDTO<object>> GetCandidateStatisticsAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var spes = new ApplicationWithVacancySpecification(fromDate, toDate);
                var applicationsRepo = _unitOfWork.Repository<Application>();
                var applications = await applicationsRepo.GetAllWithSpecAsync(spes);
                var totalApplications = applications.Count();
                var uniqueCandidates = applications.Select(a => a.CandidateId).Distinct().Count();
                var applicationsByStatus = applications
                .GroupBy(a => a.Status)
                .Select(g => new ApplicationStatusCountDto
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Percentage = Math.Round((double)g.Count() / totalApplications * 100, 2)
                })
                .OrderByDescending(x => x.Count)
                .ToList();
                var applicationsByMonth = applications
        .GroupBy(a => new { a.AppliedOn.Year, a.AppliedOn.Month })
        .Select(g => new MonthlyApplicationDto
        {
            Year = g.Key.Year,
            Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM"),
            Count = g.Count()
        })
        .OrderBy(x => x.Year)
        .ThenBy(x => DateTime.ParseExact(x.Month, "MMMM", null).Month)
        .ToList();
                var topVacancies = applications
        .GroupBy(a => new { a.VacancyId, a.Vacancy.Title })
        .Select(g => new VacancyApplicationDto
        {
            VacancyId = g.Key.VacancyId,
            VacancyTitle = g.Key.Title,
            ApplicationCount = g.Count()
        })
        .OrderByDescending(x => x.ApplicationCount)
        .Take(10)
        .ToList();
                var averageApplicationsPerCandidate = uniqueCandidates > 0
        ? Math.Round((double)totalApplications / uniqueCandidates, 2)
        : 0;
                var CandidatStatistic = new CandidateStatisticsDto
                {
                    TotalApplications = totalApplications,
                    TotalCandidates = uniqueCandidates,
                    ApplicationsByStatus = applicationsByStatus,
                    ApplicationsByMonth = applicationsByMonth,
                    TopVacancies = topVacancies,
                    AverageApplicationsPerCandidate = averageApplicationsPerCandidate
                };
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Get Candidate Statistic Succeful",
                    Data = CandidatStatistic
                };
            }
            catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }

        public async Task<ResponseDTO<object>> GetCompanyStatisticsAsync()
        {
            try
            {
               // var spes = new CompanyWithSpecification(fromDate,toDate);
                var companyRepo = _unitOfWork.Repository<Company>();
                var companies = await companyRepo.GetAllAsync();
                if (companies == null || !companies.Any())
                {
                    var CompaniesNull = new CompanyStatisticsDto
                    {
                        TotalCompanies = 0,
                        ActiveCompanies = 0,
                        CompaniesByIndustry = new List<IndustryCountDto>(),
                        CompaniesCreatedByMonth = new List<MonthlyCompanyDto>(),
                        TopCompaniesByVacancies = new List<TopCompanyDto>(),
                        TopCompaniesByApplications = new List<TopCompanyDto>()
                    };
                    return new ResponseDTO<object> {
                        IsSuccess=true,
                        Message="Get Companies With null Data",
                        Data= CompaniesNull,
                    };
                    //return

                }
                var companiesList = companies.ToList();
                var totalCompanies = companiesList.Count;
                var activeCompanies = companiesList.Count(c => c.Vacancies != null && c.Vacancies.Any(v => v.Status == "Open"));
                var companyByIndustry = companies.GroupBy(c => c.Industry ?? "Not Specified")
                        .Select(g => new IndustryCountDto
                        {
                            Industry = g.Key,
                            Count = g.Count(),
                            Percentage = Math.Round((double)g.Count() / companiesList.Count * 100, 2)
                        })
                        .OrderByDescending(x => x.Count)
                        .ToList();
              
                var compantStatistic = new CompanyStatisticsDto
                {
                    TotalCompanies = totalCompanies,
                    ActiveCompanies = activeCompanies,
                    CompaniesByIndustry= companyByIndustry
                };
               
                    return new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Message = "Company Statistis ",
                        Data = compantStatistic
                    };
            }
            catch (Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }
        public async Task<ResponseDTO<object>> GetVacancyStatisticsAsync()
        {
            try
            {
               // var spes=new VacancyWithApplicationsSpecification(fromDate, toDate);
                var vacanciesrepo = _unitOfWork.Repository<Vacancy>();
                var vacancies = await vacanciesrepo.GetAllAsync();
              
                if (vacancies == null || !vacancies.Any())
                {
                    var VacancieswithNull = new VacancyStatisticsDto
                    {
                        TotalVacancies = 0,
                        OpenVacancies =0,
                        ClosedVacancies = 0
                         
                    };
                    return new ResponseDTO<object>
                    {

                    };
                }
                var vacancieList = vacancies.ToList();
                var totalVacancies = vacancieList.Count();
                var totalApplications = vacancies.SelectMany(v => v.Applications).Count();
                var VacancyStatisc = new VacancyStatisticsDto 
                {
                    TotalVacancies = totalVacancies,
                  //  OpenVacancies = vacancies.Count(v => v.Status == "Open"),
                   // ClosedVacancies = vacancies.Count(v => v.Status == "Closed"),
                //    VacanciesByStatus = vacancies
                //.GroupBy(v => v.Status)
                //.Select(g => new VacancyStatusCountDto
                //{
                //    Status = g.Key,
                //    Count = g.Count(),
                //    Percentage = Math.Round((double)g.Count() / totalVacancies * 100, 2)
                //})
                //.OrderByDescending(x => x.Count)
                //.ToList(),
                //    VacanciesBySalaryRange = GetSalaryRangeStatistics(vacancies),
                //    AverageApplicationsPerVacancy = totalVacancies > 0
                //? Math.Round((double)totalApplications / totalVacancies, 2)
                //: 0
                };
                return new ResponseDTO<object> 
                {
                    IsSuccess=true,
                    Message="Get Vacanct Statistic Successful",
                    Data= VacancyStatisc,
                };
            }
            catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }
        private List<SalaryRangeDto> GetSalaryRangeStatistics(IEnumerable<Vacancy> vacancies)
        {
            var vacanciesWithSalary = vacancies.Where(v => v.SalaryMin.HasValue && v.SalaryMax.HasValue);
            var totalCount = vacanciesWithSalary.Count();

            if (totalCount == 0) return new List<SalaryRangeDto>();

            var ranges = new Dictionary<string, int>
        {
            {"0-30k", 0}, {"30k-50k", 0}, {"50k-70k", 0},
            {"70k-100k", 0}, {"100k+", 0}
        };

            foreach (var vacancy in vacanciesWithSalary)
            {
                var avgSalary = (vacancy.SalaryMin.Value + vacancy.SalaryMax.Value) / 2;

                if (avgSalary < 30000) ranges["0-30k"]++;
                else if (avgSalary < 50000) ranges["30k-50k"]++;
                else if (avgSalary < 70000) ranges["50k-70k"]++;
                else if (avgSalary < 100000) ranges["70k-100k"]++;
                else ranges["100k+"]++;
            }

            return ranges.Select(r => new SalaryRangeDto
            {
                Range = r.Key,
                Count = r.Value,
                Percentage = Math.Round((double)r.Value / totalCount * 100, 2)
            }).ToList();
        }

    }
}
