using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.SuperAdmin
{
    public class StatePrabhari : IEntityTypeConfiguration<Tbl_StatePrabhari>
    {


        public void Configure(EntityTypeBuilder<Tbl_StatePrabhari> builder)
        {
            builder.ToTable("Tbl_StatePrabhari");

            builder.HasQueryFilter(x => x.Status);

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.userId).IsUnique();

            builder.Property(x => x.PrabhariName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PrabhariEmail)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(x => x.Vidhansabha)
                .WithMany()
                .HasForeignKey(x => x.VidhansabhaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.State)
                .WithMany()
                .HasForeignKey(x => x.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Cast)
                .WithMany()
                .HasForeignKey(x => x.CastId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
