using HireTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Specifications
{
    public class EventNoteWithSpesification:BaseSpecifications<EventNote>
    {
        public EventNoteWithSpesification()
        {
            Includes.Add(ev=>ev.CreatedBy);
        }
    }
}
