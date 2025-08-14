using HireTech.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace HireTech.API.Extensions
{
    public static class AppConnectionString
    {
        public static void AddConnectionString( this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<HireTechDbContext>(options => 
            {
                options.UseSqlServer(configuration.GetConnectionString("HireTechConnection"));
            });
        }
    }
}
