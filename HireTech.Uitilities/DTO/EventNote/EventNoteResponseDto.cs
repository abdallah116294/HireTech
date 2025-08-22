using HireTech.Uitilities.DTO.Vacancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.EventNote
{
    public class EventNoteResponseDto
    {
        public int Id { get; set; }

        public string EntityType { get; set; }

        public int EntityId { get; set; }

        public string Description { get; set; }

        public string CreatedById { get; set; }

        // DTO for the related user, providing only essential information
        public UserBasicInfoDTO CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
