using HireTech.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Repository.Data.Configuration
{
    public class CandidateProfileConfiguration : IEntityTypeConfiguration<CandidateProfile>
    {
        public void Configure(EntityTypeBuilder<CandidateProfile> entity)
        {
            entity.ToTable("CandidateProfiles");
            entity.HasKey(entity => entity.Id);
            //Relation with user (1-to-1)
            entity.HasOne(cp => cp.User)
                .WithOne(u=>u.CandidateProfile).
                HasForeignKey<CandidateProfile>(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            //Skills (1-to-many)
            entity.HasMany(cp => cp.Skills)
                .WithOne(s => s.CandidateProfile)
                .HasForeignKey(s => s.CandidateProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            ////Applications (1-to-many)
            //entity.HasMany(cp => cp.Applications)
            //   .WithOne(a => a.CandidateProfile)
            //   .HasForeignKey(a => a.CandidateProfileId)
            //   .OnDelete(DeleteBehavior.Cascade);

            //Property Configurations
            entity.Property(cp => cp.Gender).HasMaxLength(10);
            entity.Property(cp => cp.Nationality).HasMaxLength(50);
            entity.Property(cp => cp.HighestQualification).HasMaxLength(100);
            entity.Property(cp => cp.CurrentJobTitle).HasMaxLength(100);
            entity.Property(cp => cp.CurrentCompany).HasMaxLength(100);

            entity.Property(cp => cp.CurrentSalary).HasColumnType("decimal(18,2)");
            entity.Property(cp => cp.ExpectedSalary).HasColumnType("decimal(18,2)");

            entity.Property(cp => cp.LinkedInProfile).HasMaxLength(200);
            entity.Property(cp => cp.PortfolioUrl).HasMaxLength(200);
        }
    }
}
