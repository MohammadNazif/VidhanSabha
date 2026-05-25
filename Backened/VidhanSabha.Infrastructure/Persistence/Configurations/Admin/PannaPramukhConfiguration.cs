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
    public class PannaPramukhConfiguration : IEntityTypeConfiguration<Tbl_PannaPramukh>
    {
        public void Configure(EntityTypeBuilder<Tbl_PannaPramukh> entity)
        {
            entity.ToTable("Tbl_PannaPramukh");
            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => e.Status); // ✅ global soft-delete filter

            entity.Property(e => e.PannaPramukhName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PhoneNumber).HasMaxLength(10).IsRequired();
            entity.Property(e => e.VoterId).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.ProfilePicturePath).HasMaxLength(300);
            entity.Property(e => e.Status).HasDefaultValue(true);

            // ✅ Tell EF to use private backing field
            entity.Navigation(e => e.Villages)
                  .HasField("_villages")
                  .UsePropertyAccessMode(PropertyAccessMode.Field);

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

    public class PannaPramukhVillageConfiguration : IEntityTypeConfiguration<Tbl_PannaPramukhVillage>
    {
        public void Configure(EntityTypeBuilder<Tbl_PannaPramukhVillage> entity)
        {
            entity.ToTable("Tbl_PannaPramukhVillage");
            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => e.Status);
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(e => e.PannaPramukh)
                  .WithMany(p => p.Villages)
                  .HasForeignKey(e => e.PannaPramukhId)
                  .OnDelete(DeleteBehavior.Cascade);  // ✅ changed

            entity.HasOne(e => e.Village)
                  .WithMany()
                  .HasForeignKey(e => e.VillageId)
                  .OnDelete(DeleteBehavior.Restrict); // ✅ keep Restrict here — correct
        }
    }
}
