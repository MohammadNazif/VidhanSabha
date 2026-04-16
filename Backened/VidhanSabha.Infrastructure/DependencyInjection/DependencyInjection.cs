using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using VidhanSabha.Application.Common.Cast.Interfaces;
using VidhanSabha.Application.Common.Category.Interfaces;
using VidhanSabha.Application.Common.DesignatinType.Interface;
using VidhanSabha.Application.Common.District.Interfaces;
using VidhanSabha.Application.Common.Occupation.Interface;
using VidhanSabha.Application.Common.State.Interface;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;
using VidhanSabha.Application.Pannels.Auth.Commands.Login;
using VidhanSabha.Application.Pannels.Auth.Interfaces;
using VidhanSabha.Application.Pannels.Auth.Queries.GetMobileNumber;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.Interfaces;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Interfaces;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Admin;
using VidhanSabha.Infrastructure.Repositories.Common;
using VidhanSabha.Infrastructure.Repositories.SuperAdmin;


namespace VidhanSabha.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ICategoryRepository,Categories>();
            services.AddScoped<ICastRepository, Cast>();
            services.AddScoped<IVillageRepository,Village>();
            services.AddScoped<IMandalRepository, MandalRepository>();
            services.AddScoped<ISectorRepository, SectorRepository>();
            services.AddScoped<IBoothRepository, BoothRepository>();
            services.AddScoped<IPannaPramukhRepository,PannaPramukh>();
            services.AddScoped<IDesignationType, DesignationType>();
            services.AddScoped<IDesignationRepository,DesignationRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IStateWiseVidhanSabhaCountRepository,StateWiseVidhanSabhaCountRepository>();
            services.AddScoped<IOccupationRepository, OccupationRepository>();
            services.AddScoped<IPravasiVoterRepository,PravasiVoterRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<INewVoterRepository, NewVoterRepository>();
            
            return services;
        }
    }
}