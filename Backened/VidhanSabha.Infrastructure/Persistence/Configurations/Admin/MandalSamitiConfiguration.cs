using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Admin
{
    public class MandalSamitiConfiguration : IEntityTypeConfiguration<Tbl_MandalSamiti>
    {
        public void Configure(EntityTypeBuilder<Tbl_MandalSamiti> builder)
        {
            builder.ToTable("Tbl_MandalSamiti");

            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(e => e.Status);

            builder.Property(x => x.Role).HasMaxLength(100);
            builder.Property(x => x.Status).HasDefaultValue(true);

            // ✅ MandalSamiti.MandalId → Mandal.Id (PK) — correct as-is
            builder.HasOne(x => x.Mandal)
                   .WithMany()
                   .HasForeignKey(x => x.MandalId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ✅ MandalSamiti.MandalId → MandalSanyojak.MandalId (non-PK join via shared MandalId)
            builder.HasOne(x => x.MandalSanyojak)
                   .WithMany()
                   .HasForeignKey(x => x.MandalId)
                   .HasPrincipalKey(x => x.MandalId)  // ✅ Join on MandalId, NOT on Id (PK)
                   .OnDelete(DeleteBehavior.Restrict);

            // ✅ Members: MandalSamitiMem.MandalId → MandalSamiti.Id (PK)
            builder.HasMany(x => x.Members)
                   .WithOne(x => x.MandalSamiti)
                   .HasForeignKey(x => x.MandalId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class MandalSamitiMemConfiguration : IEntityTypeConfiguration<Tbl_MandalSamitiMem>
    {
        public void Configure(EntityTypeBuilder<Tbl_MandalSamitiMem> builder)
        {
            builder.ToTable("Tbl_MandalSamitiMem");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Soft Delete Filter
            builder.HasQueryFilter(e => e.Status);

            // Properties
            builder.Property(x => x.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.Contact)
                   .HasMaxLength(50);

            builder.Property(x => x.Occupation)
                   .HasMaxLength(150);

            builder.Property(x => x.Status)
                   .HasDefaultValue(true);

            // 🔗 Navigation Properties
            builder.HasOne(x => x.MandalSamiti)
                   .WithMany(m => m.Members)
                   .HasForeignKey(x => x.MandalId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                   .WithMany()
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Caste)
                   .WithMany()
                   .HasForeignKey(x => x.CasteId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Optional: Foreign key for Designation if you add Tbl_MandalSamitiDesignation
            // builder.HasOne(x => x.Designation)
            //        .WithMany()
            //        .HasForeignKey(x => x.DesignationId)
            //        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}