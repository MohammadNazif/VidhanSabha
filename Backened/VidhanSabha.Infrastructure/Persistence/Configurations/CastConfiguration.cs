using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Infrastructure.Persistence.Configurations
{
    public class CastConfiguration : IEntityTypeConfiguration<Tbl_Cast>
    {
        public void Configure(EntityTypeBuilder<Tbl_Cast> entity)
        {
            entity.ToTable("Tbl_Cast");
            entity.HasQueryFilter(e => e.Status);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.CategoryId).IsRequired();
            entity.Property(e => e.CastName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(true);
        }
    }
}
