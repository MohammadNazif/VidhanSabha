using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.StatePrabhari;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.StatePrabhari
{
    internal class DistrictWiseCount : IEntityTypeConfiguration<Tbl_DistrictWiseCount>
    {
        public void Configure(EntityTypeBuilder<Tbl_DistrictWiseCount> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DistrictId).IsRequired();
            builder.Property(x => x.VidhansabhaCount).IsRequired();
            builder.Property(x => x.RemainingCount).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
                builder.HasOne(x => x.District)
                    .WithMany()
                    .HasForeignKey(x => x.DistrictId)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
