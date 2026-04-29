using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Infrastructure.Persistence.Configurations.SuperAdmin
{
    public class StateMembersConfiguration : IEntityTypeConfiguration<Tbl_StateMembers>
    {
        public void Configure(EntityTypeBuilder<Tbl_StateMembers> builder)
        {
            builder.ToTable("Tbl_StateMembers");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Global Filter (Soft Delete)
            builder.HasQueryFilter(x => x.Status);

            // Properties
            builder.Property(x => x.Name)
                .HasMaxLength(200);

            builder.Property(x => x.Email)
                .HasMaxLength(250);

            builder.Property(x => x.Mobile)
                .HasMaxLength(50);

            builder.Property(x => x.Profile)
                .HasColumnType("varchar(max)");

            builder.Property(x => x.Education)
                .HasMaxLength(200);

            builder.Property(x => x.Address)
                .HasColumnType("varchar(max)");

            builder.Property(x => x.Proffesion)
                .HasColumnType("varchar(max)");

            builder.Property(x => x.Status)
                .HasDefaultValue(true);

            // Relationships

            builder.HasOne(x => x.Designation)
                .WithMany()
                .HasForeignKey(x => x.DesignationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DesignationType)
                .WithMany()
                .HasForeignKey(x => x.DesignationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Cast)
                .WithMany()
                .HasForeignKey(x => x.CastId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
