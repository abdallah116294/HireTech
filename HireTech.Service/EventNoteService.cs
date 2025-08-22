using AutoMapper;
using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Core.Specifications;
using HireTech.Uitilities.DTO;
using HireTech.Uitilities.DTO.EventNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Service
{
    public class EventNoteService : IEventNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventNoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<object>> CreateEventNote(EventNoteRequestDTO dto, string userId)
        {
            try
            {
                var eventRepo = _unitOfWork.Repository<EventNote>();
                if (dto == null)
                    return new ResponseDTO<object>
                    {
                        IsSuccess = false,
                        Message = "Event Must not be null",
                        ErrorCode = ErrorCodes.BadRequest
                    };
                if (SelectInvialdType(dto.EntityType) == false)
                    return new ResponseDTO<object>
                    {
                        IsSuccess=false,
                        Message="Invaild EventType",
                    };
                var mapedEvent=_mapper.Map<EventNote>(dto);
                mapedEvent.CreatedById = userId;
                await eventRepo.AddAsync(mapedEvent);
                await _unitOfWork.CompleteAsync();
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Add Event Successful",
                    Data = mapedEvent,

                };

            }catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }

        public async Task<ResponseDTO<object>> GetAllEventNote()
        {
            try
            {
                var spes = new EventNoteWithSpesification();
                var eventRepo = _unitOfWork.Repository<EventNote>();
                var events = await eventRepo.GetAllWithSpecAsync(spes);
                if (events == null)
                    return new ResponseDTO<object>
                    {
                        IsSuccess = true,
                        Message = "Event Not null",
                        Data = null
                    };
                   var mappedEvent = _mapper.Map<IEnumerable<EventNoteResponseDto>>(events);
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = "Get All Event Successful",
                    Data = mappedEvent
                };
            }catch(Exception ex)
            {
                return new ResponseDTO<object>
                {
                    IsSuccess = true,
                    Message = $"Error Accured {ex}",
                    ErrorCode = ErrorCodes.Excptions
                };
            }
        }
        private bool SelectInvialdType(string type)
        {
            var types =new[]  { "Company", "Vacancy", "Candidate" };
            return types.Contains(type, StringComparer.OrdinalIgnoreCase);
        }
    }
}
