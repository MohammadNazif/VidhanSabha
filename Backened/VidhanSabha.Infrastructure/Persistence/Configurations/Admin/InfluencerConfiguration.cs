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
    public class InfluencerConfiguration: IEntityTypeConfiguration<Tbl_Influencer>
    {
        public void Configure(EntityTypeBuilder<Tbl_Influencer> entity)
        {
            entity.ToTable("Tbl_Influencer");
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => e.Status); // ✅ global soft-delete filter

            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();

            entity.Property(e => e.Mobile).HasMaxLength(50).IsRequired();

            entity.Property(e => e.BoothId).HasMaxLength(50).IsRequired();

            entity.Property(e => e.Description).HasMaxLength(500);

            entity.Property(e => e.CastId).HasMaxLength(50);

            entity.Property(e => e.CategoryId).HasMaxLength(50);

            entity.Property(e => e.Status).HasDefaultValue(true);

            //// ✅ Tell EF to use private backing field
            //entity.Navigation(e => e.Villages)
            //      .HasField("_villages")
            //      .UsePropertyAccessMode(PropertyAccessMode.Field);

            entity.HasOne(e => e.Booth)
                  .WithMany()
                  .HasForeignKey(e => e.BoothId)
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
    public class InfluencerVillageConfiguration : IEntityTypeConfiguration<Tbl_InfluencerVillage>
    {
        public void Configure(EntityTypeBuilder<Tbl_InfluencerVillage> entity)
        {
            entity.ToTable("Tbl_InfluencerVillage");
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => e.Status); // ✅ global soft-delete filter
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(e => e.Influencer)
                  .WithMany(i => i.Villages)
                  .HasForeignKey(e => e.InfluencerId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Village)
                  .WithMany()
                  .HasForeignKey(e => e.VillageId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
