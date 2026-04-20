using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Auth;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Auth
{
    public class CredentialMananger : IEntityTypeConfiguration<Tbl_LoginCredential>
    {
        public void Configure(EntityTypeBuilder<Tbl_LoginCredential> builder)
        {
            builder.ToTable("Tbl_LoginCredential");
            builder.HasKey(x => x.LoginId);
            builder.HasQueryFilter(x => x.Status);
            builder.Property(x => x.LoginId).HasMaxLength(50).IsRequired();
            builder.Property(x => x.UserId).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Username).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Role).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Mobile).HasMaxLength(15).IsRequired();

        }
    }
}
