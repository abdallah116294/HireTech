using HireTech.Core.Entities;
using HireTech.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IRepositories
{
    public interface IGenericRepository<T>where T : class
    {
        #region Specification
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
        Task<T> GetByIdWithSpecAsync(ISpecifications<T> Spec);
        #endregion
        //Get all
        Task<List<T>> GetAllAsync();
        //Get By Id
        Task<T> GetByIdAsync(int id);
        // Set
        Task AddAsync(T entity);
        // Update 
       // Task UpdateAsync(T entity);
        Task UpdateAsync(int id, Action<T> updateAction);
        // Delete
        Task DeleteAsync(int id);
    }
}
