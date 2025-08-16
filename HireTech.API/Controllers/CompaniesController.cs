using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace HireTech.API.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompaniesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompany(CreateCompanyDTO company) 
        {
            try
            {
                
                var companyRepo = _unitOfWork.Repository<Company>();
                var mappedCompany =_mapper.Map<Company>(company);
                mappedCompany.CreatedAt = DateTime.UtcNow;
                await companyRepo.AddAsync(mappedCompany);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<object> 
                {
                  IsSuccess=true,
                  Message="Add Company Succesful",
                  Data=company,

                });
            }catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object>
                {
                    IsSuccess = false,
                    Message = $"Error While Adding Company {ex}",
                   ErrorCode=ErrorCodes.Excptions,
                });
            }
        }
        //View Company Card  //get By it's Id
        //[Authorize(Roles = "CANDIDATE")]
        [HttpGet("GetCompanyById{id}")]
        public async Task<IActionResult> GetCompanyDetailsById(int id) 
        {
            try
            {
                var companyRepo=_unitOfWork.Repository<Company>();
                if (id == null)
                    return CreateResponse(new ResponseDTO<object> {IsSuccess=false,Message="Id is Null",ErrorCode=ErrorCodes.BadRequest });
                var companyById=await companyRepo.GetByIdAsync(id);
                if (companyById == null)
                    return CreateResponse(new ResponseDTO<object> {IsSuccess=false,Message="No Company Found",ErrorCode=ErrorCodes.NotFound});
                return CreateResponse(new ResponseDTO<object> {IsSuccess=true,Message="Get Company Details Succesful",Data=companyById });
            }
            catch (Exception ex)
            {
                return CreateResponse(new ResponseDTO<object> { IsSuccess = false, Message = $"Error While Getting Company Details {ex}",ErrorCode=ErrorCodes.Excptions});
            }
        }
        //Update company Data 
        [HttpPut("EditCompanyData{id}")]
        public async Task<IActionResult> UpdateCompanyData(int id,[FromForm]UpdateCompanyDTO company)
        {
            try
            {
                var companyRepo = _unitOfWork.Repository<Company>();
                await companyRepo.UpdateAsync(id, entity => 
                {
                    if(!string.IsNullOrEmpty(company.Name))entity.Name=company.Name;
                    if(!string.IsNullOrEmpty(company.Industry))entity.Industry=company.Industry;
                    if(!string.IsNullOrEmpty(company.Description))entity.Description=company.Description;
                      entity.CreatedAt=company.CreatedAt=DateTime.Now;
                    //if(!string.IsNullOrEmpty(company.))
                });
                await _unitOfWork.CompleteAsync();
                var ComapanyAfterUpdate = await companyRepo.GetByIdAsync(id);
                return CreateResponse(new ResponseDTO<object> {IsSuccess=true,Message="Update Company Succesful",Data=ComapanyAfterUpdate });

            }catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object> { IsSuccess =false, Message = $"Error While Update Company {ex}", ErrorCode=ErrorCodes.Excptions });
            }
        }
        //Delete Company
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteCompany(int id)
        {
            try
            {
                var companyRepo = _unitOfWork.Repository<Company>();
                if(id==null)
                    return CreateResponse(new ResponseDTO<object> { IsSuccess = false, Message = "Id is Null", ErrorCode = ErrorCodes.BadRequest });

                await companyRepo.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
                return CreateResponse(new ResponseDTO<object> { IsSuccess = true, Message = "Delete Company Succesful", });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object> { IsSuccess = false, Message = $"Error While Deleting Company {ex}", ErrorCode = ErrorCodes.Excptions });
            }
        }
        //Search by Name 
        [HttpGet("SearchByName")]
        public async Task<IActionResult> SearchByName([FromQuery] string? search)
        {
            try
            {
                var companyRepo= _unitOfWork.Repository<Company>();
                var companies=await companyRepo.GetAllAsync();
                var searchedCompany = companies.Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
                if (searchedCompany == null)
                    return CreateResponse(new ResponseDTO<object> { IsSuccess = true, Message = "No Company Found", ErrorCode = ErrorCodes.NotFound });
            return CreateResponse(new ResponseDTO<object> { IsSuccess = true, Message = "Get Company", Data=searchedCompany });
            }
            catch(Exception ex)
            {
                return CreateResponse(new ResponseDTO<object> { IsSuccess = false, Message = "Error Accured", ErrorCode = ErrorCodes.Excptions });
            }
        }
    }
}
