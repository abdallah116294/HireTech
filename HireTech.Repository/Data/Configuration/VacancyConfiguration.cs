using HireTech.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Repository.Data.Configuration
{
    public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> entity)
        {
            entity.ToTable("Vacancies");
            entity.HasKey(v => v.Id);
            entity.HasOne(v => v.Company)
                .WithMany(c=>c.Vacancies)
                .HasForeignKey(v=>v.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
 entity
    .HasOne(v => v.CreatedBy)
    .WithMany(u => u.VacanciesCreated)
    .HasForeignKey(v => v.CreatedById)
    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
