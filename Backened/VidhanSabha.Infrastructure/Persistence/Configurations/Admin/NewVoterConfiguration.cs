using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Admin
{
    public class Tbl_NewVoterConfiguration : IEntityTypeConfiguration<Tbl_NewVoter>
    {
        public void Configure(EntityTypeBuilder<Tbl_NewVoter> builder)
        {
            builder.ToTable("Tbl_NewVoter");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .HasMaxLength(200);

            builder.Property(x => x.FatherName)
                   .HasMaxLength(200);

            builder.Property(x => x.Mobile)
                   .HasMaxLength(50);

            builder.Property(x => x.VoterId)
                   .HasMaxLength(100);

            builder.Property(x => x.Status)
                   .HasDefaultValue(true);

            // 🔗 Booth FK
            builder.HasOne(e => e.Booth)
                   .WithMany()
                   .HasForeignKey(x => x.BoothId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔗 Category FK
            builder.HasOne(e => e.Category)
                   .WithMany()
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔗 Cast FK
            builder.HasOne(e => e.Cast)
                   .WithMany()
                   .HasForeignKey(x => x.CastId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
    public class Tbl_NewVoterVillageConfiguration : IEntityTypeConfiguration<Tbl_NewVoterVillage>
    {
        public void Configure(EntityTypeBuilder<Tbl_NewVoterVillage> entity)
        {
            entity.ToTable("Tbl_NewVoterVillage");

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


            entity.HasOne(e => e.NewVoter)
             .WithMany(p => p.Villages)
            .HasForeignKey(e => e.NewVoterId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
