using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Admin
{
    internal class DesignationConfiguration : IEntityTypeConfiguration<Tbl_Designation>
    {
        public DesignationConfiguration() { }

        public void Configure(EntityTypeBuilder<Tbl_Designation> entity)
        {
            entity.ToTable("Tbl_Designation");
            entity.HasQueryFilter(e => e.Status);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DesignationName)
              .IsRequired()
                .HasMaxLength(100);
           
         

        }
    }
}
