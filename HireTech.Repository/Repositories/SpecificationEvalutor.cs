using HireTech.Core.Entities;
using HireTech.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Repository.Repositories
{
    public static class SpecificationEvalutor<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> specification)
        {
            var Query = inputQuery;
            if (specification.Criteria != null)
            {
                Query = Query.Where(specification.Criteria);
            }
            //Check if OrderBy is not null and then apply it
            if (specification.OrderBy != null)
            {
                Query = Query.OrderBy(specification.OrderBy);
            }
            Query
               = specification.Includes.Aggregate(Query, (current, include) => current.Include(include));
            return Query;
        }
    }
}
