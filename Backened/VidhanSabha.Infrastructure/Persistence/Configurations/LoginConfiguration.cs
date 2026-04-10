using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Auth;

namespace VidhanSabha.Infrastructure.Persistence.Configurations
{
    public class LoginConfiguration : IEntityTypeConfiguration<Tbl_Login>
    {
        public void Configure(EntityTypeBuilder<Tbl_Login> entity)
        {
            entity.ToTable("Tbl_Login");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).UseIdentityColumn();
            entity.Property(e => e.MobileNumber).HasMaxLength(15).IsRequired();
            entity.HasIndex(e => e.MobileNumber).IsUnique();
            entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Role).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
