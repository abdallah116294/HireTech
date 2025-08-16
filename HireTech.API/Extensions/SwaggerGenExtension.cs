using Microsoft.OpenApi.Models;

namespace HireTech.API.Extensions
{
    public static class SwaggerGenExtension
    {
        public static void AddSwaggerEx(this IServiceCollection service)
        {
            service.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "HireTech API",
                        Version = "v1",
                        Description = "HireTech API with JWT Authentication"
                    });

                    // ✅ Add JWT Authentication to Swagger
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter 'Bearer' followed by a space and your token.\nExample: Bearer eyJhbGciOiJIUzI1..."
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
                }
                );


        }
    }
}
