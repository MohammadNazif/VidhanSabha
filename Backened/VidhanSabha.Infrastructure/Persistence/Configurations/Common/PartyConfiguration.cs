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
    public class PartyConfiguration: IEntityTypeConfiguration<Tbl_Party>
    {
        public void Configure(EntityTypeBuilder<Tbl_Party> entity)
        {
            entity.ToTable("Tbl_Party");
            entity.HasQueryFilter(e => e.Status);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.Party).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(true);
        }
    }
}
