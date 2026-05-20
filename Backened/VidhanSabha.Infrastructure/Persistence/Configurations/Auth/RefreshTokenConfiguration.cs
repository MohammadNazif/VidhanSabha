using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidhanSabha.Domain.Entities.Auth;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Auth
    {
        public class RefreshTokenConfiguration : IEntityTypeConfiguration<Tbl_RefreshToken>
        {
            public void Configure(EntityTypeBuilder<Tbl_RefreshToken> builder)
            {
                builder.HasKey(r => r.Id);

                builder.Property(r => r.Token).IsRequired().HasMaxLength(256);
                builder.Property(r => r.DeviceType).IsRequired().HasMaxLength(20);

                builder.HasIndex(r => r.Token).IsUnique();
                builder.HasIndex(r => new { r.UserId, r.IsRevoked });

                builder.HasOne(r => r.User)
               .WithMany(u => u.RefreshTokens)
               .HasForeignKey(r => r.UserId)
               .HasPrincipalKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Cascade);
            }
        }
}
