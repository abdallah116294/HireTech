using HireTech.Core.Entities;
using HireTech.Core.IRepositories;
using HireTech.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        //Dbcontext 
        private readonly HireTechDbContext _context;
        //Dictionary for Repository 
        private readonly Dictionary<Type, object> _repository;

        public UnitOfWork(HireTechDbContext context)
        {
            _context = context;
            _repository = new Dictionary<Type, object>();
        }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
           await _context.DisposeAsync();  
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
           //type of repository
           var type = typeof(TEntity);
            if (!_repository.ContainsKey(type))
            {
                var repositoryInstance = new GenericRepository<TEntity>(_context);
                _repository[type] = repositoryInstance;
            }
            return (GenericRepository<TEntity>)_repository[type];
        }
    }
}
