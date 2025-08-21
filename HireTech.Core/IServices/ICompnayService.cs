using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IServices
{
    public interface ICompanyService
    {
        Task<ResponseDTO<object>> GetCompanyDetailsById(int id);
        Task<ResponseDTO<object>> CreateCompany(CreateCompanyDTO dto,string userId);
        Task<ResponseDTO<object>> UpdateCompanyData(int id, UpdateCompanyDTO company);
        Task<ResponseDTO<object>> DeleteCompany(int id);
        Task<ResponseDTO<object>> SearchByName(string Name);
    }
}
