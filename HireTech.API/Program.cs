using HireTech.API.Extensions;
using HireTech.Service.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace HireTech.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerEx();
            //Connection String 
            var configuration= builder.Configuration;
            builder.Services.AddConnectionString(configuration);
            //App Service 
            builder.Services.AddAppServices(configuration);
            #region Swagger Auth

            #endregion
            #region Authentication 
            var JWTSection = configuration.GetSection("JWT");
            var secretKey = JWTSection["Key"];
            var issuer = JWTSection["ValidIssuer"];
            var audience = JWTSection["ValidAudience"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)),
                    RoleClaimType = ClaimTypes.Role
                };
            });
            #endregion
          
            var app = builder.Build();
            #region Roles Seeder 
            using (var scope = app.Services.CreateScope())
            {
                var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeederService>();
                await roleSeeder.SeedRolesAsync();
            }
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
