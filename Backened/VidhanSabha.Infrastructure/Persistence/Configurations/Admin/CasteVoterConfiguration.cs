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
    public class Tbl_CasteVoterConfiguration:IEntityTypeConfiguration<Tbl_CasteVoter>
    {
        public void Configure(EntityTypeBuilder<Tbl_CasteVoter> entity)
        {
            entity.ToTable("Tbl_CasteVoter");
            entity.HasKey(e => e.Id);
            // Soft Delete Filter
            entity.HasQueryFilter(e => e.Status);
            entity.Property(e => e.CasteVoterId)
                  .IsRequired();
            entity.Property(e => e.SubCasteId)
                  .IsRequired();
            entity.Property(e => e.Number)
                  .IsRequired();
            entity.Property(e => e.Status)
                  .HasDefaultValue(true);
            // Foreign Key Relation
            entity.HasOne(e => e.BoothVoter)  
                  .WithMany()
                  .HasForeignKey(e => e.CasteVoterId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Cast)
                  .WithMany()
                  .HasForeignKey(e => e.SubCasteId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
