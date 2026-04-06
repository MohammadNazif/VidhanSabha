using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.Auth;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Tbl_Login> Tbl_Login { get; set; }
        public DbSet<Tbl_Category> Tbl_Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tbl_Login>(entity =>
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
            });

            modelBuilder.Entity<Tbl_Category>(entity =>
            {
                entity.ToTable("tbl_category");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            modelBuilder.Entity<Tbl_Mandal>(entity =>
            {
                entity.ToTable("Tbl_Mandal");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.VidhanId).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(true);
            });
        }
    }
}
