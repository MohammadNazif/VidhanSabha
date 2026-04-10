using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations
{
    public class MandalConfiguration : IEntityTypeConfiguration<Tbl_Mandal>
    {
        public void Configure(EntityTypeBuilder<Tbl_Mandal> entity)
        {
            entity.ToTable("Tbl_Mandal");
            entity.HasQueryFilter(e => e.Status);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.VidhanId).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(true);
        }
    }
}
