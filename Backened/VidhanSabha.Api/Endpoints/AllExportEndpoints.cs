using VidhanSabha.Api.ExportExtension;
using VidhanSabha.Application.Common.ExportPdfExcel;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Interfaces;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Api.Endpoints
{
    public static class AllExportEndpointsExtensions
    {
   
        public static void AllExportEndpoints(this WebApplication app)
        {
            var pravsiExport = app.MapGroup("/api/pravasivoter")
                .RequireAuthorization(ModulePermission.PravashiVoter.ToString());

            var boothExport = app.MapGroup("/api/booth")
               .RequireAuthorization(ModulePermission.Booth.ToString());

            var doubleVoterExport = app.MapGroup("/api/doublevoter")
              .RequireAuthorization(ModulePermission.DoubleVoter.ToString());

            var newVoterExport = app.MapGroup("/api/newvoter")
       .RequireAuthorization(ModulePermission.NewVoter.ToString());

            var sahmatExport = app.MapGroup("/api/sahmat")
               .RequireAuthorization(ModulePermission.NewVoter.ToString());
            var asahmatExport = app.MapGroup("/api/asahmat")
              .RequireAuthorization(ModulePermission.NewVoter.ToString());
            // ── PDF Export ────────────────────────────────────────────────────────
            pravsiExport.MapGroup("")
              .RequireAuthorization(ModulePermission.PravashiVoter.ToString())
              .AddExportEndpoints<PravasiVoterExportRow, PravasiVoterFilter>(
               new PravasiVoterExportDef(),
              async (PravasiVoterFilter f, CancellationToken ct) =>
              {
             var repo = app.Services
                 .CreateScope().ServiceProvider
                 .GetRequiredService<IPravasiVoterRepository>();

             var qp = new PravasiQueryParams
             {
                 UserId = f.UserId,
                 SearchTerm = f.Search
             };

             var data = await repo.GetAllForExportAsync(qp, ct);

               return data.Select(m => new PravasiVoterExportRow
             {
                 Name = m.Name,
                 VoterId = m.VoterId,
                 Mobile = m.Mobile,
                 BoothNumber = m.BoothNumber,
                 Village = string.Join(", ", m.Villages.Select(v => v.VillageName)),
                 CategoryName = m.CategoryName,
                 CastName = m.CastName,
                 Occupation = m.Occupation,
                 CurrentAddress = m.CurrentAddress,
             }).ToList();
         });


            boothExport.MapGroup("")
              .RequireAuthorization(ModulePermission.Booth.ToString())
              .AddExportEndpoints<BoothExportRow, BoothFilter>(
               new BoothExportDef(),
              async (BoothFilter f, CancellationToken ct) =>
              {
                  var repo = app.Services
                      .CreateScope().ServiceProvider
                      .GetRequiredService<IBoothRepository>();

                  var qp = new BoothQueryParams
                  {
                      UserId = f.UserId,
                      SearchTerm = f.Search
                  };

                  return  await repo.GetAllForExportAsync(qp);

              });


            doubleVoterExport.MapGroup("")
              .RequireAuthorization(ModulePermission.Booth.ToString())
              .AddExportEndpoints < doublevoterExportRow, doublevoterFilter>(
               new doublevoterExportDef(),
              async (doublevoterFilter f, CancellationToken ct) =>
              {
                  var repo = app.Services
                      .CreateScope().ServiceProvider
                      .GetRequiredService<IDoubleVoterRepository>();

                  var qp = new DoubleVoterQueryParams
                  {
                      UserId = f.UserId,
                      SearchTerm = f.Search
                  };

                  return await repo.GetAllForExportAsync(qp);

              });

            newVoterExport.MapGroup("")
           .RequireAuthorization(ModulePermission.Booth.ToString())
           .AddExportEndpoints<newvoterExportRow, newvoterFilter>(
            new newvoterExportDef(),
           async (newvoterFilter f, CancellationToken ct) =>
           {
               var repo = app.Services
                   .CreateScope().ServiceProvider
                   .GetRequiredService<INewVoterRepository>();

               var qp = new NewVoterQueryParams
               {
                   UserId = f.UserId,
                   SearchTerm = f.Search
               };

               return await repo.GetAllForExportAsync(qp);

           });

            sahmatExport.MapGroup("")
         .RequireAuthorization(ModulePermission.Booth.ToString())
         .AddExportEndpoints<sahmatExportRow, sahmatFilter>(
          new sahmatExportDef(),
         async (sahmatFilter f, CancellationToken ct) =>
         {
             var repo = app.Services
                 .CreateScope().ServiceProvider
                 .GetRequiredService<ISahmatAsahmatRepository>();
             f.Type = "Sahmat";
             var qp = new SahmatAsahmatQueryParams
             {
                 UserId = f.UserId,
                 SearchTerm = f.Search,
                 Type = f.Type
              
             };

             return await repo.GetAllForExportAsync(qp);

         });
            asahmatExport.MapGroup("")
     .RequireAuthorization(ModulePermission.Booth.ToString())
     .AddExportEndpoints<sahmatExportRow, sahmatFilter>(
      new sahmatExportDef(),
     async (sahmatFilter f, CancellationToken ct) =>
     {
         var repo = app.Services
             .CreateScope().ServiceProvider
             .GetRequiredService<ISahmatAsahmatRepository>();
         f.Type = "Asahmat";
         var qp = new SahmatAsahmatQueryParams
         {
             UserId = f.UserId,
             SearchTerm = f.Search,
             Type = f.Type

         };

         return await repo.GetAllForExportAsync(qp);

     });
        }
    }

  
}
