using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.SuperAdmin
{
    public class VidhansabhaStatewiseCountConfigurations : IEntityTypeConfiguration<Tbl_VidhansabhaStatewiseCount>

    {
        public void Configure(EntityTypeBuilder<Tbl_VidhansabhaStatewiseCount> builder)
        {
            builder.ToTable("Tbl_VidhansabhaStatewiseCount");
            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(e => e.Status);
              builder.Property(x => x.Id).ValueGeneratedOnAdd();
              builder.HasOne(x => x.State).WithMany().HasForeignKey(x => x.StateId).OnDelete(DeleteBehavior.NoAction);
                builder.Property(x => x.VidhansabhaCount).IsRequired();
                builder.Property(x => x.Remainingcount).IsRequired();
                    

        }
    }
}
