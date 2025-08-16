using HireTech.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Repository.Data
{
      public class HireTechDbContext : IdentityDbContext<User>
        {
        public HireTechDbContext(DbContextOptions options):base(options)
        {
            
        }

        //Dbset Company 
        public DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Admin User",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@hiretech.com",
                NormalizedEmail = "ADMIN@HIRETECH.COM",
                EmailConfirmed = true,
                Role = "Admin",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            user.PasswordHash = hasher.HashPassword(user, "Admin@123");
            modelBuilder.Entity<User>().HasData(user);
            modelBuilder.Entity<Company>().HasData(new Company
            {
                Id = 1,
                Name = "Tech Corp",
                Industry = "IT",
                Website = "https://techcorp.com",
                Description = "A sample tech company",
                CreatedById = user.Id,
                CreatedAt = DateTime.UtcNow
            });
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HireTechDbContext).Assembly);
        }
    }
}
