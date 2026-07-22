using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Application
{
    public class CreateApplicationDTO
    {
        
        public string? CandidateId { get; set; }

        [Required]
        public int VacancyId { get; set; }

       public string? Status { get; set; }
    }
}
