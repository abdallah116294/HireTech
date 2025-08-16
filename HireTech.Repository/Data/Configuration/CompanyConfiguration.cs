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
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> entity)
        {
            entity.ToTable("Companies");
            entity.HasKey(c=>c.Id);
            // Name
            entity.Property(c=>c.Name)
                .HasColumnType("nvarchar(200)")
                .IsRequired();
            //Created At 
            entity.Property(c => c.CreatedAt)
                .HasColumnType("Date").IsRequired();
            //WebSite 
            entity.Property(c=>c.Website).HasColumnType("nvarchar(200)")
                .IsRequired();
            //Decription
            entity.Property(c=>c.Description).HasColumnType("nvarchar(200)")
                .IsRequired();
            //Industry
            entity.Property(c=>c.Industry).HasColumnType("nvarchar(200)")
                .IsRequired();
            entity
    .HasOne(c => c.CreatedBy)
    .WithMany(u => u.CompaniesCreated)
    .HasForeignKey(c => c.CreatedById)
    .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
