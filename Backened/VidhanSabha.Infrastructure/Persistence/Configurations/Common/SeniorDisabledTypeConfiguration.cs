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
    public class SeniorDisabledTypeConfiguration : IEntityTypeConfiguration<Tbl_SeniorDisabledType>
    {
        public void Configure(EntityTypeBuilder<Tbl_SeniorDisabledType> entity)
        {
            entity.ToTable("Tbl_SeniorDisabledType");
            entity.HasQueryFilter(e => e.Status);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.Type).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(true);
        }
    }
}
