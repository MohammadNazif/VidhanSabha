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
    public class Tbl_PrabhavshaliVyaktiConfiguration : IEntityTypeConfiguration<Tbl_PrabhavshaliVyakti>
    {
        public void Configure(EntityTypeBuilder<Tbl_PrabhavshaliVyakti> entity)
        {
            entity.ToTable("Tbl_PrabhavshaliVyakti");

            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.Mobile)
                .HasMaxLength(50);

            entity.Property(e => e.Description)
                .HasMaxLength(300);

            entity.Property(e => e.Status)
                .HasDefaultValue(true);


            // Foreign Keys

            entity.HasOne(e => e.Booth)
                .WithMany()
                .HasForeignKey(e => e.BoothId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Designation)
                .WithMany()
                .HasForeignKey(e => e.DesignationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Cast)
                .WithMany()
                .HasForeignKey(e => e.CastId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class Tbl_PrabhavshaliVillageConfiguration : IEntityTypeConfiguration<Tbl_PrabhavshaliVillage>
    {
        public void Configure(EntityTypeBuilder<Tbl_PrabhavshaliVillage> entity)
        {
            entity.ToTable("Tbl_PrabhavshaliVillage");

            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.Status)
                .HasDefaultValue(true);


            entity.HasOne(e => e.Village)
                .WithMany()
                .HasForeignKey(e => e.VillageId)
                .OnDelete(DeleteBehavior.Restrict);


            entity.HasOne(e => e.Prabhavshali)
                .WithMany(p => p.Villages)
                .HasForeignKey(e => e.PrabhavshaliId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
