using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Dashboard
{
    public class VacancyApplicationDto
    {
        public int VacancyId { get; set; }
        public string VacancyTitle { get; set; }
        public int ApplicationCount { get; set; }
    }
}
