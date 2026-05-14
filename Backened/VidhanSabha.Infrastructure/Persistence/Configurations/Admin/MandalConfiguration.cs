using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Admin;
using static VidhanSabha.Domain.Entities.Admin.Tbl_Mandal;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Admin
{
    public class MandalConfiguration : IEntityTypeConfiguration<Tbl_Mandal>
    {
        public void Configure(EntityTypeBuilder<Tbl_Mandal> entity)
        {
            entity.ToTable("Tbl_Mandal");
            entity.HasQueryFilter(e => e.Status);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.VidhanId).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(e => e.Sanyojak)
                .WithMany()
                .HasForeignKey(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public class MandalSanyojakConfiguration : IEntityTypeConfiguration<Tbl_MandalSanyojak>
        {
            public void Configure(EntityTypeBuilder<Tbl_MandalSanyojak> entity)
            {
                entity.ToTable("Tbl_MandalSanyojak");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InchargeName).HasMaxLength(150).IsRequired();
                entity.Property(e => e.FatherName).HasMaxLength(150).IsRequired();
                entity.Property(e => e.PhoneNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(300);
                entity.Property(e => e.ProfileImagePath).HasMaxLength(500);
                entity.HasIndex(e => e.MandalId).IsUnique();

                // ✅ Correct one-to-one relationship configured from FK side
                entity.HasOne(e => e.Mandal)
                      .WithOne(m => m.Sanyojak)
                      .HasForeignKey<Tbl_MandalSanyojak>(e => e.MandalId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Cast)
                      .WithMany()
                      .HasForeignKey(e => e.CastId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Navigation(e => e.Cast).AutoInclude(false);
            }
        }
    }
}
