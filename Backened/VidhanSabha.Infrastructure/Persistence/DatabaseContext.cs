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

            //public DbSet<Tbl_Login> Tbl_Login => Set<Tbl_Login>();
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
        public DbSet<Tbl_District>Tbl_District=>Set<Tbl_District>();

        public DbSet<Tbl_VidhansabhaStatewiseCount> Tbl_VidhansabhaStatewiseCount => Set<Tbl_VidhansabhaStatewiseCount>();
        public DbSet<Tbl_Occupation> Tbl_Occupation => Set<Tbl_Occupation>();
        public DbSet<Tbl_PravasiVoter> Tbl_PravasiVoter => Set<Tbl_PravasiVoter>();
        public DbSet<Tbl_PravasiVillage> Tbl_PravasiVillage => Set<Tbl_PravasiVillage>();
        public DbSet<Tbl_NewVoter> Tbl_NewVoter => Set<Tbl_NewVoter>();
        public DbSet<Tbl_NewVoterVillage> Tbl_NewVoterVillage => Set<Tbl_NewVoterVillage>();
        public DbSet<Tbl_Party> Tbl_Party => Set<Tbl_Party>();
        public DbSet<Tbl_SahmatType> Tbl_SahmatType =>Set<Tbl_SahmatType>();
        public DbSet<Tbl_SahmatAsahmat> Tbl_SahmatAsahmat => Set<Tbl_SahmatAsahmat>();
        public DbSet<Tbl_SahmatAsahmatVillage> Tbl_SahmatAsahmatVillage => Set<Tbl_SahmatAsahmatVillage>();

        public DbSet<Tbl_StatePrabhari> Tbl_StatePrabhari => Set<Tbl_StatePrabhari>();

        public DbSet<Tbl_LoginCredential> Tbl_LoginCredential => Set<Tbl_LoginCredential>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // ✅ Ek line — is assembly ki saari Configuration classes auto-pick ho jayengi
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

                base.OnModelCreating(modelBuilder);
            }
        }
    }
