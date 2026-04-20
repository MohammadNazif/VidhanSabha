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
    public class PradhanConfiguration : IEntityTypeConfiguration<Tbl_Pradhan>
    {
        public void Configure(EntityTypeBuilder<Tbl_Pradhan> builder)
        {
            builder.ToTable("Tbl_Pradhan");

            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(e => e.Status);

            builder.Property(x => x.Name)
                   .HasMaxLength(200);

            builder.Property(x => x.Contact)
                   .HasMaxLength(50);

            builder.Property(x => x.Gender)
                   .HasMaxLength(50);

            builder.Property(x => x.Status)
                   .HasDefaultValue(true);

            // 🔗 Designation FK
            //builder.HasOne(e => e.DesignationId)
            //       .WithMany()
            //       .HasForeignKey(x => x.DesignationId)
            //       .OnDelete(DeleteBehavior.Restrict);


        }
    }
    public class Tbl_PradhanVillageConfiguration : IEntityTypeConfiguration<Tbl_PradhanVillage>
    {
        public void Configure(EntityTypeBuilder<Tbl_PradhanVillage> entity)
        {
            entity.ToTable("Tbl_PradhanVillage");

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


            entity.HasOne(e => e.Pradhan)
             .WithMany(p => p.Villages)
            .HasForeignKey(e => e.PradhanId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
