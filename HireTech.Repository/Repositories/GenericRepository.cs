using HireTech.Core.IRepositories;
using HireTech.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HireTechDbContext _context;

        public GenericRepository(HireTechDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
           await  _context.Set<T>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity=await GetByIdAsync(id);
             _context.Set<T>().Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var entities = await _context.Set<T>().ToListAsync();
            return entities;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }

    

        public async Task UpdateAsync(int id, Action<T> updateAction)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                updateAction(entity);
                _context.Set<T>().Update(entity);
            }
            else
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }
        }

        //public Task UpdateAsync(T entity)
        //{
        //    _context.Set<T>().Update(entity);
        //   // throw new NotImplementedException();
        //}

      
    }
}
