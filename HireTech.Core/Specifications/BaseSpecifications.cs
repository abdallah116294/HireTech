using HireTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set ; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get ; set ; }
        //Get All
        public BaseSpecifications()
        {

        }
        // Get By ID 
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
            //  Includes = new List<Expression<Func<T, object>>>();
        }

    }
}
