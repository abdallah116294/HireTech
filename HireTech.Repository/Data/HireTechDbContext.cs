using HireTech.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Repository.Data
{
    public class HireTechDbContext : DbContext
    {
        public HireTechDbContext(DbContextOptions options):base(options)
        {
            
        }
        //Dbset Company 
        public DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HireTechDbContext).Assembly);
        }
    }
}
