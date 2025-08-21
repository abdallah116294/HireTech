using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IServices
{
    public interface IProfileService
    {
        Task<ResponseDTO<object>> GetProfile(string userId);
        Task<ResponseDTO<object>> AddProfile(CreateCandidateProfileDTO dto,string userId);
        Task<ResponseDTO<object>> UpdateProfile(UpdateCandidateProfileDTO dto,string userId);
    }
}
