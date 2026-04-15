using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.Auth;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Infrastructure.Persistence
{
    
        public class DatabaseContext : DbContext
        {
            public DatabaseContext(DbContextOptions<DatabaseContext> options)
                : base(options) { }

            public DbSet<Tbl_Login> Tbl_Login => Set<Tbl_Login>();
            public DbSet<Tbl_Category> Tbl_Category => Set<Tbl_Category>();
            public DbSet<Tbl_Mandal> Tbl_Mandal => Set<Tbl_Mandal>();
            public DbSet<Tbl_Cast> Tbl_Cast => Set<Tbl_Cast>();
            public DbSet<Tbl_Village> Tbl_Village => Set<Tbl_Village>();
            public DbSet<Tbl_Sector> Tbl_Sector => Set<Tbl_Sector>();
            public DbSet<Tbl_Booth> Tbl_Booth => Set<Tbl_Booth>();
            public DbSet<Tbl_BoothVillage> Tbl_BoothVillage => Set<Tbl_BoothVillage>();
            public DbSet<Tbl_BoothSanyojak> Tbl_BoothSanyojak => Set<Tbl_BoothSanyojak>();

            // Naye modules add honge to sirf DbSet add karo — OnModelCreating touch nahi karna
            public DbSet<Tbl_PannaPramukh> Tbl_PannaPramukh => Set<Tbl_PannaPramukh>();
            public DbSet<Tbl_PannaPramukhVillage> Tbl_PannaPramukhVillage => Set<Tbl_PannaPramukhVillage>();

           public DbSet<Tbl_DesignationType> Tbl_DesignationType => Set<Tbl_DesignationType>();

         public DbSet<Tbl_Designation> Tbl_Designation => Set<Tbl_Designation>();

        public DbSet<Tbl_State> Tbl_State => Set<Tbl_State>();

        public DbSet<Tbl_VidhansabhaStatewiseCount> Tbl_VidhansabhaStatewiseCount => Set<Tbl_VidhansabhaStatewiseCount>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // ✅ Ek line — is assembly ki saari Configuration classes auto-pick ho jayengi
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

                base.OnModelCreating(modelBuilder);
            }
        }
    }
