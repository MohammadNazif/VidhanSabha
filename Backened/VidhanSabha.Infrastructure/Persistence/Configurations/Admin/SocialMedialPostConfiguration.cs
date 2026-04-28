using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.Admin
{
    public class Tbl_SocialMediaPostConfiguration : IEntityTypeConfiguration<Tbl_SocialMediaPost>
    {
        public void Configure(EntityTypeBuilder<Tbl_SocialMediaPost> entity)
        {
            entity.ToTable("Tbl_SocialMediaPost");

            // Primary Key
            entity.HasKey(e => e.Id);

            // Global Filter (Soft Delete)
            entity.HasQueryFilter(e => e.Status);

            // Properties
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false) // because varchar
                .IsRequired(false);

            entity.Property(e => e.PostImagePath)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Description)
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Status)
                .HasDefaultValue(true);
            
        }
    }

    public class Tbl_SocialPostPlatformConfiguration : IEntityTypeConfiguration<Tbl_SocialPostPlatform>
    {
        public void Configure(EntityTypeBuilder<Tbl_SocialPostPlatform> entity)
        {
            entity.ToTable("Tbl_SocialPostPlatform");

            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.Status)
                .HasDefaultValue(true);

            entity.HasOne(e => e.Platform)
                .WithMany()
                .HasForeignKey(e => e.PlatformId);

            entity.HasOne(e => e.SocialMediaPost)
                .WithMany(p => p.Platforms) // 👈 collection in Post
                .HasForeignKey(e => e.SocialMediaPostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }


    public class Tbl_SocialMediaBoothConfiguration : IEntityTypeConfiguration<Tbl_SocialMediaBooth>
    {
        public void Configure(EntityTypeBuilder<Tbl_SocialMediaBooth> entity)
        {
            entity.ToTable("Tbl_SocialMediaBooth");

            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.Status)
                .HasDefaultValue(true);

            entity.HasOne(e => e.Booth)
                .WithMany()
                .HasForeignKey(e => e.BoothId);

            entity.HasOne(e => e.SocialMediaPost)
                .WithMany(p => p.Booths) // 👈 collection in Post
                .HasForeignKey(e => e.SocialMediaPostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class Tbl_SocialMediaSectorConfiguration : IEntityTypeConfiguration<Tbl_SocialMediaSector>
    {
        public void Configure(EntityTypeBuilder<Tbl_SocialMediaSector> entity)
        {
            entity.ToTable("Tbl_SocialMediaSector");

            entity.HasKey(e => e.Id);

            entity.HasQueryFilter(e => e.Status);

            entity.Property(e => e.Status)
                .HasDefaultValue(true);

            entity.HasOne(e => e.Sector)
                .WithMany()
                .HasForeignKey(e => e.SectorId);

            entity.HasOne(e => e.SocialMediaPost)
                .WithMany(p => p.Sectors) // 👈 collection in Post
                .HasForeignKey(e => e.SocialMediaPostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class Tbl_SocialMediaPlatformConfiguration : IEntityTypeConfiguration<Tbl_SocialMediaPlatform>
    {
        public void Configure(EntityTypeBuilder<Tbl_SocialMediaPlatform> entity)
        {
            entity.ToTable("Tbl_SocialMediaPlatform");

            // Primary Key
            entity.HasKey(e => e.Id);

            // Global Filter
            entity.HasQueryFilter(e => e.Status);

            // Properties
            entity.Property(e => e.Platform)
                .HasMaxLength(200)
                .IsUnicode(false) // varchar
                .IsRequired(false);

            entity.Property(e => e.Status)
                .HasDefaultValue(true);
        }
    }
}
