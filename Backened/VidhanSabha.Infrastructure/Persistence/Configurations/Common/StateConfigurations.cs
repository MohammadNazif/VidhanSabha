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
    public class StateConfigurations : IEntityTypeConfiguration<Tbl_State>
    {
        public void Configure(EntityTypeBuilder<Tbl_State> builder)
        {
            builder.ToTable("Tbl_State");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            builder.Property(e => e.StateName)
                .IsRequired()
                .HasColumnName("StateName");

        }
    }
}
