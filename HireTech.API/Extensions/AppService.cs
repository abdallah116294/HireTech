using HireTech.Core.IRepositories;
using HireTech.Repository.Repositories;

namespace HireTech.API.Extensions
{
    public static class AppService
    {
        public static void AddAppServices(this IServiceCollection service)
        {
            // Repository 
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Unit of work
            service.AddScoped<IUnitOfWork, IUnitOfWork>();
        }
    }
}
