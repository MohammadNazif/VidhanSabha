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

            builder.HasOne(x => x.Category)
        .WithMany()
        .HasForeignKey(x => x.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Caste)
                   .WithMany()
                   .HasForeignKey(x => x.CasteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Contact)
                   .HasMaxLength(50);

            builder.Property(x => x.Occupation)
                   .HasMaxLength(150);

            builder.Property(x => x.Status)
                   .HasDefaultValue(true);

            // 🔗 Designation FK 
            builder.HasOne(x => x.Designation)
       .WithMany()
       .HasForeignKey(x => x.DesignationId)
       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
