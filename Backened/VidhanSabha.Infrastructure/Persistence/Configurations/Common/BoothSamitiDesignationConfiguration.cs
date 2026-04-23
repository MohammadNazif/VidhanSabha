using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Common
{
    public class BoothSamitiDesignationConfiguration
    : IEntityTypeConfiguration<Tbl_BoothSamitiDesignation>
    {
        public void Configure(EntityTypeBuilder<Tbl_BoothSamitiDesignation> builder)
        {
            builder.ToTable("Tbl_BoothSamitiDesignation");

            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => x.Status);

            builder.Property(x => x.DesignationName)
                   .HasMaxLength(200);
        }
    }
}
