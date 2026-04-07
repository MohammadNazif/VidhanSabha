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

        public DbSet<Tbl_Mandal> Tbl_Mandal { get; set; }

        public DbSet<Tbl_Cast> Tbl_Cast { get; set; }

        public DbSet<Tbl_Village> Tbl_Village { get; set; }

         public DbSet<Tbl_Sector> Tbl_Sector { get; set; }

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
                entity.HasQueryFilter(e => e.Status);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.VidhanId).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(true);
            });
            modelBuilder.Entity<Tbl_Cast>(entity =>
            {
                entity.ToTable("Tbl_Cast");
                entity.HasQueryFilter(e => e.Status);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.CategoryId).IsRequired();
                entity.Property(e => e.CastName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(true);
            });
            modelBuilder.Entity<Tbl_Village>(entity =>
            {
                entity.ToTable("Tbl_Village");
                entity.HasQueryFilter(e => e.Status);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.VillageName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            modelBuilder.Entity<Tbl_Sector>(entity =>
            {
                entity.ToTable("Tbl_Sector");
                entity.HasQueryFilter(e => e.Status);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();

                // Required fields
                entity.Property(e => e.SectorName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.IsSectorSanyojak).HasDefaultValue(false);
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.CreatedById).IsRequired();

                // Sanyojak optional fields
                entity.Property(e => e.InchargeName).HasMaxLength(100);
                entity.Property(e => e.FatherName).HasMaxLength(100);
                entity.Property(e => e.EducationLevel).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(10);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.ProfileImage).HasMaxLength(500);
                entity.Property(e => e.Age);
                entity.Property(e => e.CategoryId);
                entity.Property(e => e.CastId);

                // Audit fields
                entity.Property(e => e.Status).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt);

                // Foreign Keys
                entity.HasOne(e => e.Mandal)
                      .WithMany()
                      .HasForeignKey(e => e.MandalId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
