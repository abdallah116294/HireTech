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
    public class EventNoteConfiguartion : IEntityTypeConfiguration<EventNote>
    {
        public void Configure(EntityTypeBuilder<EventNote> entity)
        {
            entity.ToTable("Events");
            entity.HasKey(e => e.Id);
           entity
    .HasOne(e => e.CreatedBy)
    .WithMany(u => u.EventsCreated)
    .HasForeignKey(e => e.CreatedById)
    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
