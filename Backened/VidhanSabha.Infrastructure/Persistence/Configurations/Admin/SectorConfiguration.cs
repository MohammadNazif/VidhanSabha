using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Admin
{
    public class SectorConfiguration : IEntityTypeConfiguration<Tbl_Sector>
    {
        public void Configure(EntityTypeBuilder<Tbl_Sector> entity)
        {
            entity.ToTable("Tbl_Sector");
            entity.HasQueryFilter(e => e.Status);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.SectorName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.IsSectorSanyojak).HasDefaultValue(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedById).IsRequired();

            // Sanyojak optional fields
            entity.Property(e => e.InchargeName).HasMaxLength(100);
            entity.Property(e => e.FatherName).HasMaxLength(100);
            entity.Property(e => e.EducationLevel).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ProfileImage).HasMaxLength(500);

            // Audit
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // FK
            entity.HasOne(s => s.Mandal)
                  .WithMany(m => m.Sectors)
                  .HasForeignKey(s => s.MandalId)  
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
