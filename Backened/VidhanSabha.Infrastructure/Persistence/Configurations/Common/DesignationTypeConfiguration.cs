using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Common
{
    public class DesignationTypeConfiguration : IEntityTypeConfiguration<Tbl_DesignationType>
    {
        public void Configure(EntityTypeBuilder<Tbl_DesignationType> entity)
        {
            entity.ToTable("Tbl_DesignationType");
            entity.HasQueryFilter(e => e.Status);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            //entity.Property(e => e.VillageName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(true);

        }
    }
}
