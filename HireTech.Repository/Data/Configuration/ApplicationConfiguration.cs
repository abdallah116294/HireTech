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
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> entity)
        {
            entity.ToTable("Applications");
            entity.HasKey(p => p.Id);
         entity
     .HasOne(a => a.Vacancy)
     .WithMany(v => v.Applications)
     .HasForeignKey(a => a.VacancyId)
     .OnDelete(DeleteBehavior.Cascade);

           entity
                .HasOne(a => a.Candidate)
                .WithMany() 
                .HasForeignKey(a => a.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
