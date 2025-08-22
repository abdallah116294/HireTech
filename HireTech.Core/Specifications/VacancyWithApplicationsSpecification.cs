using HireTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Specifications
{
    public class VacancyWithApplicationsSpecification:BaseSpecifications<Vacancy>
    {
        public VacancyWithApplicationsSpecification(int id):base(v=>v.Id==id)
        {
            Includes.Add(v => v.Applications);
        }
        public VacancyWithApplicationsSpecification(DateTime fromDate, DateTime toDate):base(v => v.CreatedAt >= fromDate && v.CreatedAt <= toDate)
        {
            
        }
    }
}
