using DocumentFormat.OpenXml.Spreadsheet;
using VidhanSabha.Api.ExportExtension;
using VidhanSabha.Application.Common.ExportPdfExcel;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos.VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.BDC.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Influencer.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Pradhan.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;
using VidhanSabha.Domain.Enums;
using static VidhanSabha.Application.Common.ExportPdfExcel.Dtos.BDCExportDef;
using static VidhanSabha.Application.Common.ExportPdfExcel.Dtos.BlockExportDef;
using static VidhanSabha.Application.Common.ExportPdfExcel.Dtos.InfluencerExportDef;

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

            var sectorExport = app.MapGroup("/api/sector")
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

            var pannapramukhExport = app.MapGroup("/api/pannapramukh")
                .RequireAuthorization();
            var mandalReportExport = app.MapGroup("/api/mandalreport")
             .RequireAuthorization();

            var combinedMandalReportExport = app.MapGroup("/api/combinedmandalreport")
             .RequireAuthorization();

            var mandalExport = app.MapGroup("/api/mandal")
             .RequireAuthorization();

            var boothReportExport = app.MapGroup("/api/boothreport")
            .RequireAuthorization();

            var sectorReportExport = app.MapGroup("/api/sectorreport")
            .RequireAuthorization();

            var blockExport = app.MapGroup("/api/block")
            .RequireAuthorization();
            var bdcExport = app.MapGroup("/api/bdc")
            .RequireAuthorization();

            var influencerExport = app.MapGroup("/api/influencer")
            .RequireAuthorization();

            var boothSamitiMemExport = app.MapGroup("/api/boothsamitimembers")
            .RequireAuthorization();

            var boothSamitiExport = app.MapGroup("/api/boothsamiti")
            .RequireAuthorization();

            var sectorWithBoothReportExport = app.MapGroup("/api/sectorwithboothreport")
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

                  return await repo.GetAllForExportAsync(qp);

                });

              // sectorExport.MapGroup("")
              //.RequireAuthorization()
              //.AddExportEndpoints<SectorExportRow, SectorFilter>(
              // new SectorExportDef(),
              //async (SectorFilter f, CancellationToken ct) =>
              //{
              //    var repo = app.Services
              //        .CreateScope().ServiceProvider
              //        .GetRequiredService<ISectorRepository>();

              //    var qp = new SectorQueryParams
              //    {
              //        UserId = f.UserId,
              //        SearchTerm = f.Search
              //    };

              //    //return await repo.GetAllForExportAsync(qp);

              //});


            doubleVoterExport.MapGroup("")
              .RequireAuthorization()
              .AddExportEndpoints<doublevoterExportRow, doublevoterFilter>(
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
          new sahmatExportDef("Sahmat List"),
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
      new sahmatExportDef("Asahmat List"),
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
            new prabhavsaliExportDef("Prabhavsali List"),
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
      new prabhavsaliExportDef("Doctor's List"),
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
new prabhavsaliExportDef("Advocate's List"),
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
new prabhavsaliExportDef("Goverment Employees List"),
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
.AddExportEndpoints<PradhanExportRow, PradhanExportFilter>(
new PradhanExportDef(),
async (PradhanExportFilter f, CancellationToken ct) =>
{
    var repo = app.Services
              .CreateScope().ServiceProvider
              .GetRequiredService<IPradhanRepository>();
    var qp = new PradhanExportFilter
    {
        UserId = f.UserId,
        SearchTerm = f.Search,

    };

    return await repo.GetPradhanExportAsync(qp);

});


 seniorcitizenExport.MapGroup("")
.RequireAuthorization()
.AddExportEndpoints<seniordisabledExportRow, seniordisabledFilter>(
new seniordisabledExportDef("VaristhNagarik List"),
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
new seniordisabledExportDef("ViklangNagarik List"),
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

            mandalReportExport.MapGroup("")
.RequireAuthorization()
.AddExportEndpoints<MandalReportExportRow, MandalReportFilter>(
new MandalReportExportDef(),
async (MandalReportFilter f, CancellationToken ct) =>
{

    var repo = app.Services
              .CreateScope().ServiceProvider
              .GetRequiredService<IMandalRepository>();

    var qp = new MandalQueryParams
    {
        UserId = f.UserId,
        SearchTerm = f.Search,
        //TypeId = f.TypeId,

    };
    int? vidhanId = await repo.GetVidhansabhaIdByuserIdAsync(qp.UserId);
    return await repo.GetAllMandalReportsForExport(qp, vidhanId);

});
        //    combinedMandalReportExport.MapGroup("")
        //     .RequireAuthorization()
        //    .AddExportEndpoints<CombinedReportExportRow, CombinedReportFilter>(
        //new CombinedReportExportDef(),
        //async (CombinedReportFilter f, CancellationToken ct) =>
        //{

        //    var repo = app.Services
        //       .CreateScope().ServiceProvider
        //       .GetRequiredService<IMandalRepository>();

        //    var qp = new CombinedReportFilter
        //    {
        //        UserId = f.UserId,

        //    };
        //    int? vidhanId = await repo.GetVidhansabhaIdByuserIdAsync(qp.UserId);
        //    return await repo.GetAllCombinedMandalReportsExp(qp, vidhanId);

        //});


            mandalExport.MapGroup("")
.RequireAuthorization()
.AddExportEndpoints<MandalExportRow, MandalFilter>(
new MandalExportDef(),
async (MandalFilter f, CancellationToken ct) =>
{

    var repo = app.Services
              .CreateScope().ServiceProvider
              .GetRequiredService<IMandalRepository>();

    var qp = new MandalFilter
    {
        UserId = f.UserId,
        SearchTerm = f.Search,
        //TypeId = f.TypeId,

    };
    int? vidhanId = await repo.GetVidhansabhaIdByuserIdAsync(qp.UserId);
    return await repo.GetMandalExportAsync(qp, vidhanId);

});

            boothReportExport.MapGroup("")
           .RequireAuthorization()
.AddExportEndpoints<BoothReportExportRow, BoothReportFilter>(
new BoothReportExportDef(),
async (BoothReportFilter f, CancellationToken ct) =>
{

    var repo = app.Services
                  .CreateScope().ServiceProvider
                  .GetRequiredService<IBoothRepository>();

    var qp = new BoothReportFilter
    {
        UserId = f.UserId,

    };
    return await repo.GetBoothReportExportAsync(qp);

});

            sectorReportExport.MapGroup("")
           .RequireAuthorization()
           .AddExportEndpoints<SectorReportExportRow, SectorReportFilter>(
new SectorReportExportDef(),
async (SectorReportFilter f, CancellationToken ct) =>
{

    var repo = app.Services
               .CreateScope().ServiceProvider
               .GetRequiredService<ISectorRepository>();

    var qp = new SectorReportFilter
    {
        UserId = f.UserId,
        //SearchTerm = f.Search,
        //TypeId = f.TypeId,

    };
    return await repo.GetSectorReportExportAsync(qp);

   });

            combinedMandalReportExport.MapGroup("")
         .RequireAuthorization()
         .AddExportEndpoints<CombinedReportExportRow, CombinedReportFilter>(
         new CombinedReportExportDef(),
         async (CombinedReportFilter f, CancellationToken ct) =>
       {

              var repo = app.Services
             .CreateScope().ServiceProvider
             .GetRequiredService<IMandalRepository>();

     var qp = new CombinedReportFilter
     {
      UserId = f.UserId,
      //SearchTerm = f.Search,
      //TypeId = f.TypeId,

     };
           int? vidhanId = await repo.GetVidhansabhaIdByuserIdAsync(qp.UserId);
           return await repo.GetAllCombinedMandalReportsExp(qp,vidhanId);

      });


            blockExport.MapGroup("")
         .RequireAuthorization()
         .AddExportEndpoints<BlockExportRow, BlockExportFilter>(
         new BlockExportDef(),
         async (BlockExportFilter f, CancellationToken ct) =>
         {

             var repo = app.Services
            .CreateScope().ServiceProvider
            .GetRequiredService<IBlockRepository>();

             var qp = new BlockExportFilter
             {
                 UserId = f.UserId,
                

             };
             
             return await repo.GetBlockExportAsync(qp);

         });

            bdcExport.MapGroup("")
       .RequireAuthorization()
       .AddExportEndpoints<BDCExportRow, BDCExportFilter>(
       new BDCExportDef(),
       async (BDCExportFilter f, CancellationToken ct) =>
       {

           var repo = app.Services
          .CreateScope().ServiceProvider
          .GetRequiredService<IBDCRepository>();

           var qp = new BDCExportFilter
           {
               UserId = f.UserId,


           };

           return await repo.GetBDCExportAsync(qp);

       });

            influencerExport.MapGroup("")
       .RequireAuthorization()
       .AddExportEndpoints<InfluencerExportRow, InfluencerExportFilter>(
       new InfluencerExportDef(),
       async (InfluencerExportFilter f, CancellationToken ct) =>
       {

           var repo = app.Services
          .CreateScope().ServiceProvider
          .GetRequiredService<IInfluencerRepository>();

           var qp = new InfluencerExportFilter
           {
               UserId = f.UserId,
           };

           return await repo.GetInfluencerExportAsync(qp);

       });

            boothSamitiMemExport.MapGroup("")
    .RequireAuthorization()
    .AddExportEndpoints<BoothSamitiMemberExportRow, BoothSamitiMemberFilter>(
     new BoothSamitiMemberExportDef("Booth Samiti List"),
    async (BoothSamitiMemberFilter f, CancellationToken ct) =>
    {

        var repo = app.Services
       .CreateScope().ServiceProvider
       .GetRequiredService<IBoothSamitiRepository>();

        var qp = new BoothSamitiMemberFilter
        {
           BoothMemId = f.BoothMemId
        };

        return await repo.GetAllMemberForExportAsync(qp);

    });

            boothSamitiExport.MapGroup("")
    .RequireAuthorization()
    .AddExportEndpoints<BoothSamitiExportRow, BoothSamitiFilter>(
        new BoothSamitiExportDef("Booth Samiti List"),
        async (BoothSamitiFilter f, CancellationToken ct) =>
        {
            var repo = app.Services
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<IBoothSamitiRepository>();

            var qp = new BoothSamitiMemberFilter
            {
                UserId = f.UserId
            };

            return await repo.GetAllSamitiForExportAsync(f, ct);
        });

            sectorWithBoothReportExport.MapGroup("")
    .RequireAuthorization()
    .AddExportEndpoints<SectorWithBoothReportExportRow, SectorWithBoothReportExportFilter>(
        new SectorWithBoothReportExportDef("Sector With Booth Report"),
        async (SectorWithBoothReportExportFilter f, CancellationToken ct) =>
        {
            var repo = app.Services
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<ISectorRepository>();
            var mandal = app.Services
              .CreateScope()
              .ServiceProvider
              .GetRequiredService<IMandalRepository>();

            var qp = new SectorWithBoothReportExportFilter
            {
                UserId = f.UserId
            };
            int? vidhanId = await mandal.GetVidhansabhaIdByuserIdAsync(qp.UserId);
            return await repo.GetAllSectorWithBoothReportsForExportAsync(qp, vidhanId, ct);
        });

        }
    }


}
