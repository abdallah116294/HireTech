using HireTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Specifications
{
    public class CompanyWithSpecification:BaseSpecifications<Company>
    {
     public   CompanyWithSpecification()
        {
            Includes.Add(c => c.Vacancies);
        }
        public CompanyWithSpecification(int id):base(c=>c.Id==id)
        {
            Includes.Add(c => c.Vacancies);  
        }
        public CompanyWithSpecification(DateTime fromDate, DateTime toDate) :base(c => c.CreatedAt >= fromDate && c.CreatedAt <= toDate)
        {
            
        }
    }
}
