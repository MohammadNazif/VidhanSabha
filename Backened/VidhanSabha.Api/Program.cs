using System.Data;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VidhanSabha.Api.Endpoints;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Query;
using VidhanSabha.Application.Common.MemberModulePermission.Command;
using VidhanSabha.Application.Pannels.Auth.DTOs;
using VidhanSabha.Application.Pannels.Auth.Interfaces;
using VidhanSabha.Domain.Enums;
using VidhanSabha.Infrastructure.DependencyInjection;
using VidhanSabha.Infrastructure.Persistence;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200","http://localhost:8081", "https://localhost:8081", "http://localhost:8082", "https://localhost:8082","https://matdaancrm.in")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Register service
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthorizationHandler, ModulePermissionHandler>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });
builder.Services.AddAuthorization(options =>
{
    foreach (ModulePermission module in Enum.GetValues(typeof(ModulePermission)))
    {
        options.AddPolicy(module.ToString(), policy =>
        {
            var captured = module; // capture for closure
            policy.Requirements.Add(new ModulePermissionRequirement(captured));
        });
    }
});

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token like this: Bearer {your token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// Open generic explicit registration
builder.Services.AddAllExportHandlers(
    typeof(GenericExportQuery<,>).Assembly,  
    typeof(Program).Assembly);

var app = builder.Build();
app.UseCors("AllowAngular");
app.UseExceptionHandler();



    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapSuperAdminEndpoints();
app.MapAdminEndpoints();
app.MapCommonEndpoints();
app.AllExportEndpoints();
app.MapAuthEndpoints();

app.MapControllers();
app.UseStaticFiles();

app.MapFallbackToFile("index.html");
app.Run();
