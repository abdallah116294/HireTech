using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Company
{
    public  class CreateCompanyDTO
    {
        public string Name { get; set; }
        public string Industry { get; set; }
        [Url(ErrorMessage ="Pleas enter valid URl for websit ")]
        public string Website { get; set; }
        public string Description { get; set; }
        //will be an FK to user who created id 
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}
