using VidhanSabha.Api.ExportExtension;
using VidhanSabha.Application.Common.ExportPdfExcel;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
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


        }
    }

  
}
