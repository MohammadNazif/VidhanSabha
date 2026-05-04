using VidhanSabha.Api.ExportExtension;
using VidhanSabha.Application.Common.ExportPdfExcel;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Api.Endpoints
{
    public static class AllExportEndpointsExtensions
    {
   
        public static void AllExportEndpoints(this WebApplication app)
        {
            var pravsiExport = app.MapGroup("/api/pravasivoter")
                .RequireAuthorization();

            var boothExport = app.MapGroup("/api/booth")
               .RequireAuthorization();

            var doubleVoterExport = app.MapGroup("/api/doublevoter")
              .RequireAuthorization();

            var newVoterExport = app.MapGroup("/api/newvoter")
       .RequireAuthorization();

            var sahmatExport = app.MapGroup("/api/sahmat")
               .RequireAuthorization();
            var asahmatExport = app.MapGroup("/api/asahmat")
              .RequireAuthorization();

            var doctorExport = app.MapGroup("/api/doctor")
           .RequireAuthorization();
            var advocateExport = app.MapGroup("/api/advocate")
       .RequireAuthorization();
            var governmentemoloyeeExport = app.MapGroup("/api/governmentemoloyee")
       .RequireAuthorization();

            var pradhanExport = app.MapGroup("/api/pradhan")
  .RequireAuthorization();

            var prabhavsaliExport = app.MapGroup("/api/prabhavshali")
.RequireAuthorization();

            var seniorcitizenExport = app.MapGroup("/api/seniorcitizen")
.RequireAuthorization();

            var disabledExport = app.MapGroup("/api/disabled")
.RequireAuthorization();

         var   pannapramukhExport = app.MapGroup("/api/pannapramukh")
          .RequireAuthorization();



            // ── PDF Export ────────────────────────────────────────────────────────
            pravsiExport.MapGroup("")
              .RequireAuthorization()
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
              .RequireAuthorization()
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
              .RequireAuthorization()
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
           .RequireAuthorization()
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
         .RequireAuthorization()
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
     .RequireAuthorization()
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

            prabhavsaliExport.MapGroup("")
     .RequireAuthorization()
     .AddExportEndpoints<prabhavsaliExportRow, prabhavsaliFilter>(
      new prabhavsaliExportDef(),
     async (prabhavsaliFilter f, CancellationToken ct) =>
     {
         var repo = app.Services
             .CreateScope().ServiceProvider
             .GetRequiredService<IPrabhavshaliRepository>();
         f.designationId = null;
         var qp = new PrabhavshaliQueryParams
         {
             UserId = f.UserId,
             SearchTerm = f.Search,
             designationId = f.designationId,

         };

         return await repo.GetExportByDesgIdAsync(qp);

     });

            doctorExport.MapGroup("")
     .RequireAuthorization()
     .AddExportEndpoints<prabhavsaliExportRow, prabhavsaliFilter>(
      new prabhavsaliExportDef(),
     async (prabhavsaliFilter f, CancellationToken ct) =>
     {
         var repo = app.Services
             .CreateScope().ServiceProvider
             .GetRequiredService<IPrabhavshaliRepository>();
         f.designationId = 8;
         var qp = new PrabhavshaliQueryParams
         {
             UserId = f.UserId,
             SearchTerm = f.Search,
             designationId = f.designationId,

         };

         return await repo.GetExportByDesgIdAsync(qp);

     });

            advocateExport.MapGroup("")
.RequireAuthorization()
.AddExportEndpoints<prabhavsaliExportRow, prabhavsaliFilter>(
new prabhavsaliExportDef(),
async (prabhavsaliFilter f, CancellationToken ct) =>
{
  var repo = app.Services
      .CreateScope().ServiceProvider
      .GetRequiredService<IPrabhavshaliRepository>();
  f.designationId = 9;
  var qp = new PrabhavshaliQueryParams
  {
      UserId = f.UserId,
      SearchTerm = f.Search,
      designationId = f.designationId,

  };

  return await repo.GetExportByDesgIdAsync(qp);

});
          

            governmentemoloyeeExport.MapGroup("")
.RequireAuthorization()
.AddExportEndpoints<prabhavsaliExportRow, prabhavsaliFilter>(
new prabhavsaliExportDef(),
async (prabhavsaliFilter f, CancellationToken ct) =>
{
var repo = app.Services
        .CreateScope().ServiceProvider
        .GetRequiredService<IPrabhavshaliRepository>();
f.designationId = 10;
var qp = new PrabhavshaliQueryParams
{
UserId = f.UserId,
SearchTerm = f.Search,
designationId = f.designationId,

};

return await repo.GetExportByDesgIdAsync(qp);

});


            pradhanExport.MapGroup("")
.RequireAuthorization()
.AddExportEndpoints<prabhavsaliExportRow, prabhavsaliFilter>(
new prabhavsaliExportDef(),
async (prabhavsaliFilter f, CancellationToken ct) =>
{
  var repo = app.Services
            .CreateScope().ServiceProvider
            .GetRequiredService<IPrabhavshaliRepository>();
  f.designationId = 1;
     var qp = new PrabhavshaliQueryParams
  {
      UserId = f.UserId,
      SearchTerm = f.Search,
      designationId = f.designationId,

  };

  return await repo.GetExportByDesgIdAsync(qp);

});


            seniorcitizenExport.MapGroup("")
.RequireAuthorization()
.AddExportEndpoints<seniordisabledExportRow, seniordisabledFilter>(
new seniordisabledExportDef(),
async (seniordisabledFilter f, CancellationToken ct) =>
{
    var repo = app.Services
              .CreateScope().ServiceProvider
              .GetRequiredService<ISeniorDisabledRepository>();
       f.TypeId = 1;
    var qp = new SeniorDisabledQueryParams
    {
        UserId = f.UserId,
        SearchTerm = f.Search,
        TypeId = f.TypeId,

    };

       return await repo.GetSeniorDisabledExportAsync(qp);

});


            disabledExport.MapGroup("")
.RequireAuthorization()
.AddExportEndpoints<seniordisabledExportRow, seniordisabledFilter>(
new seniordisabledExportDef(),
async (seniordisabledFilter f, CancellationToken ct) =>
{
    var repo = app.Services
              .CreateScope().ServiceProvider
              .GetRequiredService<ISeniorDisabledRepository>();
    f.TypeId = 2;
    var qp = new SeniorDisabledQueryParams
    {
        UserId = f.UserId,
        SearchTerm = f.Search,
        TypeId = f.TypeId,

    };

    return await repo.GetSeniorDisabledExportAsync(qp);

});

            pannapramukhExport.MapGroup("")
.RequireAuthorization()
.AddExportEndpoints<PannaPramukhExportRow, PannaPramukhFilter>(
new PannaPramukhExportDef(),
async (PannaPramukhFilter f, CancellationToken ct) =>
{
  var repo = app.Services
            .CreateScope().ServiceProvider
            .GetRequiredService<IPannaPramukhRepository>();
 
  var qp = new PannaPramukhQueryParams
  {
      UserId = f.UserId,
      SearchTerm = f.Search,
      //TypeId = f.TypeId,

  };

  return await repo.GetPannaPramukhExportAsync(qp);

});
        }
    }

  
}
