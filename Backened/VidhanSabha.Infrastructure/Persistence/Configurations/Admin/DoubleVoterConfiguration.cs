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
    public class Tbl_DoubleVoterConfiguration : IEntityTypeConfiguration<Tbl_DoubleVoter>
    {
        public void Configure(EntityTypeBuilder<Tbl_DoubleVoter> entity)
        {
            entity.ToTable("Tbl_DoubleVoter");

            entity.HasKey(e => e.Id);

            // Soft Delete Filter
            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.BoothId)
                  .IsRequired();

            entity.Property(e => e.Name)
                  .HasMaxLength(200);

            entity.Property(e => e.FatherName)
                  .HasMaxLength(200);

            entity.Property(e => e.VoterId)
                  .HasMaxLength(200);

            entity.Property(e => e.PreviousAddress)
                  .HasMaxLength(300);

            entity.Property(e => e.CurrentAddress)
                  .HasMaxLength(300);

            entity.Property(e => e.Description)
                  .HasMaxLength(300);

            entity.Property(e => e.Status)
                  .HasDefaultValue(true);

            // Foreign Key Relation
            entity.HasOne(e => e.Booth)
                  .WithMany()
                  .HasForeignKey(e => e.BoothId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
    public class Tbl_DoubleVillageConfiguration : IEntityTypeConfiguration<Tbl_DoubleVoterVillage>
    {
        public void Configure(EntityTypeBuilder<Tbl_DoubleVoterVillage> entity)
        {
            entity.ToTable("Tbl_DoubleVoterVillage");
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => e.Status);
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(e => e.Village)
                .WithMany()
                .HasForeignKey(e => e.VillageId);

            entity.HasOne(e => e.Double)
                .WithMany(p => p.Villages)
                .HasForeignKey(e => e.DoubleVoterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

    
