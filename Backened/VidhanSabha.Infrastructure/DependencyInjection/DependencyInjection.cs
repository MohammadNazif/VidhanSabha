using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VidhanSabha.Application.Common.Cast.Interfaces;
using VidhanSabha.Application.Common.Category.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;
using VidhanSabha.Application.Pannels.Auth.Commands.Login;
using VidhanSabha.Application.Pannels.Auth.Interfaces;
using VidhanSabha.Application.Pannels.Auth.Queries.GetMobileNumber;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Admin;
using VidhanSabha.Infrastructure.Repositories.Common;


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

            return services;
        }
    }
}