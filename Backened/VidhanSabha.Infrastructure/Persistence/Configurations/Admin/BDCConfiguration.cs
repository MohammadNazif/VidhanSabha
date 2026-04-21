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
    public class Tbl_BDCConfiguration : IEntityTypeConfiguration<Tbl_BDC>
    {
        public void Configure(EntityTypeBuilder<Tbl_BDC> entity)
        {
            entity.ToTable("Tbl_BDC");

            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => e.Status);


            // Properties

            entity.Property(e => e.Block)
                .HasMaxLength(200);

            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.WardNumber)
                .HasMaxLength(200);

            entity.Property(e => e.Mobile)
                .HasMaxLength(50);

            entity.Property(e => e.Education)
                .HasMaxLength(200);

            entity.Property(e => e.Age)
                .IsRequired(true);

            entity.Property(e => e.Status)
                .HasDefaultValue(true);


            // Foreign Keys

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Cast)
                .WithMany()
                .HasForeignKey(e => e.CastId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Party)
                .WithMany()
                .HasForeignKey(e => e.PartyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class Tbl_BDCVillageConfiguration : IEntityTypeConfiguration<Tbl_BDCVillage>
    {
        public void Configure(EntityTypeBuilder<Tbl_BDCVillage> entity)
        {
            entity.ToTable("Tbl_BDCVillage");

            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.Status)
                .HasDefaultValue(true);


            // Foreign Keys

            entity.HasOne(e => e.BDC)
                .WithMany(p => p.Villages)
                .HasForeignKey(e => e.BDCId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Village)
                .WithMany()
                .HasForeignKey(e => e.VillageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
