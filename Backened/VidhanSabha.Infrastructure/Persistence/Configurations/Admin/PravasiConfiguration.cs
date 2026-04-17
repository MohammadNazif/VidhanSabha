using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Admin;
namespace VidhanSabha.Infrastructure.Persistence.Configurations.Admin 
{ public class Tbl_PravasiVoterConfiguration : IEntityTypeConfiguration<Tbl_PravasiVoter> 
    { public void Configure(EntityTypeBuilder<Tbl_PravasiVoter> entity) 
        { entity.ToTable("Tbl_PravasiVoter");  
            entity.HasKey(e => e.Id);  
            entity.HasQueryFilter(e => e.Status);  
            entity.Property(e => e.Name) .HasMaxLength(200) .IsRequired(); 
            entity.Property(e => e.Mobile) .HasMaxLength(50); 
            entity.Property(e => e.VoterId) .HasMaxLength(100); 
            entity.Property(e => e.CurrentAddress) .HasMaxLength(300); 
            entity.Property(e => e.Status) .HasDefaultValue(true);  

            entity.HasOne(e => e.Booth) 
                .WithMany() 
                .HasForeignKey(e => e.BoothId) 
                .OnDelete(DeleteBehavior.Restrict); 

            entity.HasOne(e => e.Category) 
                .WithMany() 
                .HasForeignKey(e => e.CategoryId) 
                .OnDelete(DeleteBehavior.Restrict); 

            entity.HasOne(e => e.Cast) 
                .WithMany() 
                .HasForeignKey(e => e.CastId) 
                .OnDelete(DeleteBehavior.Restrict); 

            entity.HasOne(e => e.Occupation) 
                .WithMany() 
                .HasForeignKey(e => e.OccupationId) 
                .OnDelete(DeleteBehavior.Restrict); 
        } 
    } 
    public class Tbl_PravasiVillageConfiguration : IEntityTypeConfiguration<Tbl_PravasiVillage> 
    { public void Configure(EntityTypeBuilder<Tbl_PravasiVillage> entity) 
        { entity.ToTable("Tbl_PravasiVillage");  
            entity.HasKey(e => e.Id); 
            entity.HasQueryFilter(e => e.Status); 
            entity.Property(e => e.Status) .HasDefaultValue(true); 

            entity.HasOne(e => e.Village) 
                .WithMany() 
                .HasForeignKey(e => e.VillageId);
            
            entity.HasOne(e => e.Pravasi) 
                .WithMany(p => p.Villages) 
                .HasForeignKey(e => e.PravasiId) 
                .OnDelete(DeleteBehavior.Cascade);
        } 
    } 
}