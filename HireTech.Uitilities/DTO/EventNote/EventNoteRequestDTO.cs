using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.EventNote
{
    public class EventNoteRequestDTO
    {
        public string EntityType { get; set; }

        public int EntityId { get; set; }

        public string Description { get; set; }
    }
}
