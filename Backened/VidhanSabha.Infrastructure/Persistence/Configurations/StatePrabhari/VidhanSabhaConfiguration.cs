using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.StatePrabhari;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.StatePrabhari
{
    internal class VidhanSabhaConfiguration : IEntityTypeConfiguration<Tbl_VidhanSabha>
    {
        public void Configure(EntityTypeBuilder<Tbl_VidhanSabha> builder)
        {
            builder.ToTable("Tbl_VidhanSabha");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).HasMaxLength(200).IsRequired();
            builder.Property(x => x.VidhanSabhaName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.VidhanSabhaNumber).IsRequired();
            builder.Property(x => x.DistrictId).IsRequired();
          
        }
    }
}
