using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Admin
{
    public class BoothSamitiConfiguration : IEntityTypeConfiguration<Tbl_BoothSamiti>
    {
        public void Configure(EntityTypeBuilder<Tbl_BoothSamiti> builder)
        {
            builder.ToTable("Tbl_BoothSamiti");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Soft Delete Filter
            builder.HasQueryFilter(e => e.Status);

            // Properties
            builder.Property(x => x.Name)
                   .HasMaxLength(200);

            builder.Property(x => x.Category)
                   .HasMaxLength(100);

            builder.Property(x => x.Caste)
                   .HasMaxLength(100);

            builder.Property(x => x.Contact)
                   .HasMaxLength(50);

            builder.Property(x => x.Occupation)
                   .HasMaxLength(150);

            builder.Property(x => x.Status)
                   .HasDefaultValue(true);

            // 🔗 Designation FK 
            builder.HasOne(e => e.Designation)
                   .WithMany()
                   .HasForeignKey(x => x.DesignationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
