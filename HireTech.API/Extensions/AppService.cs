using HireTech.Core.IRepositories;
using HireTech.Core.IServices;
using HireTech.Repository.Repositories;
using HireTech.Service.UserService;
using HireTech.Uitilities.DTO.User;
using HireTech.Uitilities.Helpers;
using Microsoft.Extensions.Configuration;

namespace HireTech.API.Extensions
{
    public static class AppService
    {
        public static void AddAppServices(this IServiceCollection service,IConfiguration configuration)
        {
            // Repository 
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Unit of work
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            //AutoMapper
            service.AddAutoMapper(typeof(MappingProfile));
            //User Repository
            service.AddScoped<IUserRepository, UserRepository>();
            service.Configure<EmailConfiguration>(configuration.GetSection("EmialConfiguration"));
            var emailConfig = configuration.GetSection("EmialConfiguration").Get<EmailConfiguration>();
            service.AddSingleton(emailConfig); // Register EmailConfiguration as a singleton service
            service.AddScoped<IEmailService, EmailService>(); // Register the email service    
            service.AddTransient<TokenHelper>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<RoleSeederService>();
            service.AddScoped<IRoleRepository, RoleRepository>();
            service.AddScoped<IRoleService, RoleService>();
        }
    }
}
