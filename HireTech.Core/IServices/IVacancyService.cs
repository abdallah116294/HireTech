using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.Vacancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IServices
{
    public interface IVacancyService
    {
        Task<ResponseDTO<object>> CreateVacancy(CreateVacancyDTO dto,string userId);
        Task<ResponseDTO<object>> GetVacancyById(int id);
        Task<ResponseDTO<object>> GetAllVacancies();
        Task<ResponseDTO<object>> GetCandidatesApplication(int id);
        Task<ResponseDTO<object>> UpdateVacancy(int id, UpdateVacancyDTO dto,string userId);
        Task<ResponseDTO<object>> AccecptCandidate(string Status,string candidateId,int vacancyId, int ApplicationId);
    }
}
