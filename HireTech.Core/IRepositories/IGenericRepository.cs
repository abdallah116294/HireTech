using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.IRepositories
{
    public interface IGenericRepository<T>where T : class
    {
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
