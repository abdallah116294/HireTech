using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.EventNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IServices
{
    public interface IEventNoteService
    {
        Task<ResponseDTO<object>> CreateEventNote(EventNoteRequestDTO dto, string userId);
        Task<ResponseDTO<object>> GetAllEventNote();
    }
}
