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
    public class Tbl_BoothVoterConfiguration : IEntityTypeConfiguration<Tbl_BoothVoter>
    {
        public void Configure(EntityTypeBuilder<Tbl_BoothVoter> builder)
        {
            builder.ToTable("Tbl_BoothVoter");

            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(e => e.Status);

            builder.Property(x => x.BoothId)
                   .HasMaxLength(200);

            builder.Property(x => x.TotalVoter)
                   .HasMaxLength(200);

            builder.Property(x => x.Male)
                   .HasMaxLength(50);

            builder.Property(x => x.Female)
                   .HasMaxLength(50);

            builder.Property(x => x.Other)
                   .HasMaxLength(50);

            builder.Property(x => x.Status)
                   .HasDefaultValue(true);

            // 🔗 Booth FK
            builder.HasOne(e => e.Booth)
                   .WithMany()
                   .HasForeignKey(x => x.BoothId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
    public class Tbl_BoothVoterVillageConfiguration : IEntityTypeConfiguration<Tbl_BoothVoterVillage>
    {
        public void Configure(EntityTypeBuilder<Tbl_BoothVoterVillage> entity)
        {
            entity.ToTable("Tbl_BoothVoterVillage");

            // Primary Key
            entity.HasKey(e => e.Id);

            // Soft Delete Filter
            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.Status)
                  .HasDefaultValue(true);

            // Relationships

            entity.HasOne(e => e.Village)
                  .WithMany()
                  .HasForeignKey(e => e.VillageId);


            entity.HasOne(e => e.BoothVoter)
             .WithMany(p => p.Villages)
            .HasForeignKey(e => e.BoothVoterId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
