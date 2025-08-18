using HireTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Specifications
{
    public interface ISpecifications<T>where T:class
    {
        //Signture of Property of Query 
        //Dbcontext => start query (Entry point)
        //Where Conditions 
        public Expression<Func<T, bool>> Criteria { get; set; }
        //Include query 
        public List<Expression<Func<T, object>>> Includes { get; set; }
        //order by
        public Expression<Func<T, object>> OrderBy { get; set; }

    }
}
