using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Entities
{
    public class EventNote:BaseEntity
    {
        // Type of entity this event belongs to
        public string EntityType { get; set; }
        // Company, Vacancy, Candidate, Message

        public int EntityId { get; set; }

        public string Description { get; set; }

        // User who created it
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
