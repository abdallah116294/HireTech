using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Entities
{
    public class Company: BaseEntity
    {
        //Company Name
        public string Name { get; set; }
        public string Industry { get; set; }
        public string  Website { get; set; }
        public string  Description { get; set; }
        //will be an FK to user who created id 
        public string  CreatedById  { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
