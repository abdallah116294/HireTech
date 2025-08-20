using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Application
{
    public class ApplicationResponseDTO
    {
        public int Id { get; set; }
        public string CandidateId { get; set; }
        public int VacancyId { get; set; }
        public string Status { get; set; }
        public DateTime AppliedOn { get; set; }
        public string VacancyTitle { get; set; }
        public string CompanyName { get; set; }
    }
}
