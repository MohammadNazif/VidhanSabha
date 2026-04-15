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
    public class BoothConfiguration : IEntityTypeConfiguration<Tbl_Booth>
    {
        public void Configure(EntityTypeBuilder<Tbl_Booth> entity)
        {
            entity.ToTable("Tbl_Booth");
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.MandalId).IsRequired();
            entity.Property(e => e.SectorId).IsRequired();
            entity.Property(e => e.BoothNumber).IsRequired();
            entity.Property(e => e.PollingStationName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.PollingStationLocation).HasMaxLength(300).IsRequired();
            entity.Property(e => e.IsBoothSanyojak)
                  .HasDefaultValue(false)
                  .IsRequired()
                  .ValueGeneratedNever();

            entity.HasIndex(e => new { e.MandalId, e.BoothNumber }).IsUnique();

            entity.HasOne(e => e.Sanyojak)
                  .WithOne()
                  .HasForeignKey<Tbl_BoothSanyojak>(s => s.BoothId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class BoothVillageConfiguration : IEntityTypeConfiguration<Tbl_BoothVillage>
    {
        public void Configure(EntityTypeBuilder<Tbl_BoothVillage> entity)
        {
            entity.ToTable("Tbl_BoothVillage");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VillageId).IsRequired();
            entity.Property(e => e.HasAnshik).IsRequired();

            entity.HasOne(e => e.Booth)
                  .WithMany(b => b.Villages)
                  .HasForeignKey(e => e.BoothId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Village)
                  .WithMany()
                  .HasForeignKey(e => e.VillageId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Navigation(e => e.Village).AutoInclude(false);
        }
    }
    public class BoothSanyojakConfiguration : IEntityTypeConfiguration<Tbl_BoothSanyojak>
    {
        public void Configure(EntityTypeBuilder<Tbl_BoothSanyojak> entity)
        {
            entity.ToTable("Tbl_BoothSanyojak");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.InchargeName).HasMaxLength(150).IsRequired();
            entity.Property(e => e.FatherName).HasMaxLength(150).IsRequired();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.ProfileImagePath).HasMaxLength(500);

            entity.HasIndex(e => e.BoothId).IsUnique();

            entity.HasOne(e => e.Cast)
                  .WithMany()
                  .HasForeignKey(e => e.CastId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Navigation(e => e.Cast).AutoInclude(false);
        }
    }
}

