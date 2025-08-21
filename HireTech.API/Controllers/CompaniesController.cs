using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Core.Specifications;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace HireTech.API.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;

        public CompaniesController(IUnitOfWork unitOfWork, IMapper mapper, ICompanyService companyService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _companyService = companyService;
        }
        [Authorize(Roles = "RECRUITER")]
        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompany(CreateCompanyDTO dto) 
        {
            var userId = User.FindFirstValue("id"); // logged-in user ID
            Console.WriteLine(userId);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User is not logged in" });
            var result = await _companyService.CreateCompany(dto, userId);
            return CreateResponse(result);
        }
        //View Company Card  //get By it's Id
        [Authorize(Roles = "CANDIDATE,RECRUITER")]
        [HttpGet("GetCompanyById{id}")]
        public async Task<IActionResult> GetCompanyDetailsById(int id) 
        {
            var result = await _companyService.GetCompanyDetailsById(id);
            return CreateResponse(result);
        }
        //Update company Data 
        [HttpPut("EditCompanyData{id}")]
        public async Task<IActionResult> UpdateCompanyData(int id,[FromForm]UpdateCompanyDTO company)
        {
            var result = await _companyService.UpdateCompanyData(id, company);
            return CreateResponse(result);
            
        }
        //Delete Company
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteCompany(int id)
        {
            var result=await _companyService.DeleteCompany(id);
            return CreateResponse(result);
        }
        //Search by Name 
        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName([FromQuery] string? search)
        {
            var result = await _companyService.SearchByName(search);
            return CreateResponse(result);
        }
    }
}
