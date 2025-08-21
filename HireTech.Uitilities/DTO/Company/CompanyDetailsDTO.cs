using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Uitilities.DTO.Company
{
    public class CompanyDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string  CreateById { get; set; }
      //  public string CreateBy { get; set; }
        public List<VacancyDto> Vacancies { get; set; }
    }
    public class VacancyDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Status { get; set; }
    }
}
