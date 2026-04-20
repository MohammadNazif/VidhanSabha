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
    public class Tbl_BlockConfiguration : IEntityTypeConfiguration<Tbl_Block>
    {
        public void Configure(EntityTypeBuilder<Tbl_Block> entity)
        {
            entity.ToTable("Tbl_Block");

            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.BlockName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.BlockPramukh)
                .HasMaxLength(200);

            entity.Property(e => e.Mobile)
                .HasMaxLength(50);

            entity.Property(e => e.Address);

            //entity.Property(e => e.Profile);

            entity.Property(e => e.Status)
                .HasDefaultValue(true);


            entity.HasOne(e => e.Party)
                .WithMany()
                .HasForeignKey(e => e.PartyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Cast)
                .WithMany()
                .HasForeignKey(e => e.CastId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Occupation)
                .WithMany()
                .HasForeignKey(e => e.OccupationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
