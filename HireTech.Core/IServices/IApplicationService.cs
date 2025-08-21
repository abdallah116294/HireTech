using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IServices
{
    public interface IApplicationService
    {
        Task<ResponseDTO<object>> AddApplication(CreateApplicationDTO createDto,string userId);
        Task<ResponseDTO<object>> GetAllCandidateApplication(string usrId);
    }
}
