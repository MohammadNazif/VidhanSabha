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
    public class OccupationConfiguration:IEntityTypeConfiguration<Tbl_Occupation>
    {
        public void Configure(EntityTypeBuilder<Tbl_Occupation> entity)
        {
            entity.ToTable("Tbl_Occupation");
            entity.HasQueryFilter(e => e.Status);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.Occupation).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(true);
        }
    }
}
