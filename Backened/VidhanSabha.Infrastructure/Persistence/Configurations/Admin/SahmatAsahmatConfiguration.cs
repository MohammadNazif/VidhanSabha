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
    public class SahmatAsahmatConfiguration: IEntityTypeConfiguration<Tbl_SahmatAsahmat>
    {
        public void Configure(EntityTypeBuilder<Tbl_SahmatAsahmat> entity)
        {
            entity.ToTable("Tbl_SahmatAsahmat");

            // ✅ Primary Key
            entity.HasKey(e => e.Id);

            // ✅ Soft Delete
            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.Status)
                  .HasDefaultValue(true);

            // ✅ Properties
            entity.Property(e => e.Name)
                  .HasMaxLength(200);

            entity.Property(e => e.Mobile)
                  .HasMaxLength(50);

            entity.Property(e => e.Reason)
                  .HasMaxLength(300);

            entity.Property(e => e.VoterId)
                  .HasMaxLength(100);

            entity.Property(e => e.IsAsahmat)
                  .HasDefaultValue(false);

            entity.Property(e => e.Age);

            // 🔥 Relationships (Same as PravasiVoter style)

            entity.HasOne(e => e.Booth)
                  .WithMany()
                  .HasForeignKey(e => e.BoothId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Type)
                  .WithMany()
                  .HasForeignKey(e => e.TypeId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Party)
                  .WithMany()
                  .HasForeignKey(e => e.PartyId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Occupation)
                  .WithMany()
                  .HasForeignKey(e => e.OccupationId)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }

    public class Tbl_SahmatAsahmatVillageConfiguration
    : IEntityTypeConfiguration<Tbl_SahmatAsahmatVillage>
    {
        public void Configure(EntityTypeBuilder<Tbl_SahmatAsahmatVillage> entity)
        {
            entity.ToTable("Tbl_SahmatAsahmatVillage");

            // ✅ Primary Key
            entity.HasKey(e => e.Id);

            // ✅ Soft Delete
            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.Status)
                  .HasDefaultValue(true);

            // 🔥 Relationships

            entity.HasOne(e => e.Village)
                  .WithMany()
                  .HasForeignKey(e => e.VillageId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.SahmatAsahmat)
                  .WithMany(s => s.Villages)   
                  .HasForeignKey(e => e.SahmatAsahmatId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
