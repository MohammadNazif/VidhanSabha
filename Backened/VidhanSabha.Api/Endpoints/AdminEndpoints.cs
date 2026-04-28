using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Common.BoothSamitiDesignation.DTOs;
using VidhanSabha.Application.Common.BoothSamitiDesignation.Queries;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.BDC.Command;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;
using VidhanSabha.Application.Pannels.Admin.BDC.Queries;
using VidhanSabha.Application.Pannels.Admin.Block.Command;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.Block.Queries;
using VidhanSabha.Application.Pannels.Admin.Booth.Command;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Queries;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Command;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Queries;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.Command;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.Queries;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.Command;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.Queries;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Command;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Queries;
using VidhanSabha.Application.Pannels.Admin.Influencer.Command;
using VidhanSabha.Application.Pannels.Admin.Influencer.DTOs;
using VidhanSabha.Application.Pannels.Admin.Influencer.Queries;
using VidhanSabha.Application.Pannels.Admin.Mandal.Commands;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Queries;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Command;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Queries;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Queries;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Command;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Queries;
using VidhanSabha.Application.Pannels.Admin.Pradhan.Command;
using VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs;
using VidhanSabha.Application.Pannels.Admin.Pradhan.Queries;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Queries;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Command;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Queries;
using VidhanSabha.Application.Pannels.Admin.Sector.Commands;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Queries;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Command;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Queries;
using VidhanSabha.Domain.Enums;
using static System.Net.WebRequestMethods;
public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this WebApplication app)
    {
        var mandal = app.MapGroup("/api/mandal")
                        .WithTags("Mandal");

        var sector = app.MapGroup("/api/sector")
                       .WithTags("Sector");

        var booth = app.MapGroup("/api/booth")
                     .WithTags("Booth");

        var admin = app.MapGroup("/api/admin")
                        .WithTags("Admin");

        var pannapramukh = app.MapGroup("/api/pannapramukh")
                        .WithTags("PannaPramukh");
        var pravasivoter = app.MapGroup("/api/pravasivoter")
                        .WithTags("PravasiVoter");
        var newvoter = app.MapGroup("/api/newvoter")
                        .WithTags("NewVoter");
        var sahmatasahmat = app.MapGroup("/api/sahmatasahmat")
                        .WithTags("SahmatAsahmat");

        var pradhan = app.MapGroup("/api/pradhan")
                        .WithTags("Pradhan");

        var doublevoter = app.MapGroup("/api/doublevoter")
                        .WithTags("DoubleVoter");
        var prabhavshali = app.MapGroup("/api/prabhavshali")
                        .WithTags("PrabhavShali");
        var block = app.MapGroup("/api/block")
                        .WithTags("Block");

        var boothSamiti = app.MapGroup("/api/boothsamiti")
                    .WithTags("BoothSamiti");

        var bdc = app.MapGroup("/api/bdc")
                        .WithTags("BDC");
        var seniordisabled = app.MapGroup("/api/seniordisabled")
                        .WithTags("SeniorDisabled");


        var boothSamitiDesignation = app.MapGroup("/api/boothsamiti-designation")
                                    .WithTags("BoothSamitiDesignation");

        var influencer = app.MapGroup("/api/influencer")
                        .WithTags("Influencer");

        var boothvoter = app.MapGroup("/api/boothvoter")
                        .WithTags("BoothVoter");

        var castevoter = app.MapGroup("/api/castevoter")
                        .WithTags("CasteVoter");

        #region Mandal
        

        mandal.MapPost("/create", async (
            CreateMandalRequestDto request,
            IMediator mediator,HttpContext httpContext ) =>
        {
            string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(
                new CreateMandalCommand(request.Name,userId));

            return Results.Created($"/api/mandal/{result.Id}",
                ApiResponse<MandalResponseDto>.Ok(result));
        })
        .WithName("CreateMandal")
        .Produces<ApiResponse<MandalResponseDto>>(201)
        .Produces(400);

        mandal.MapPost("/update", async (UpdateMandalRequestDto request, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateMandalCommand
            {
                Id = request.Id,
                Name = request.Name
            });

            return Results.Ok(ApiResponse<MandalResponseDto>.Ok(result, "Mandal Updated Succesfully"));
        })
       .WithName("UpdateMandal")
       .Produces<ApiResponse<MandalResponseDto>>(200);

        mandal.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteMandalCommand(id));

            return Results.Ok(ApiResponse<int>.Ok(result, "Mandal Deleted Succesfully"));
        })
         .WithName("DeleteMandal")
         .Produces<int>(200);

        mandal.MapGet("/getAll", async (
            [AsParameters] MandalQueryParams q,
            IMediator mediator, HttpContext httpContext) =>
        {
            string UserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new GetAllMandalsQuery(q,UserId));

            return Results.Ok(ApiResponse<PagedResult<MandalResponseDto>>.Ok(result));
        }).RequireAuthorization()
        .WithName("GetAllMandals")
        .Produces<ApiResponse<PagedResult<MandalResponseDto>>>(200);

        mandal.MapGet("/getAllCombinedReports", async (
            [AsParameters] MandalQueryParams q,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllCombinedMandalReportsQuery(q));

            return Results.Ok(ApiResponse<PagedResult<MandalFullDto>>.Ok(result));
        });

        #endregion

        #region Sector
        sector.MapPost("/create", async (CreateSectorRequestDto dto, IMediator mediator, HttpContext http) =>
        {
            var userId = 1;
            var userName = "Admin";

            var result = await mediator.Send(new CreateSectorCommand(dto, userId, userName));
            return Results.Ok(ApiResponse<SectorResponseDto>.Ok(result, "Sector Created Successfully"));
        })
          .WithName("CreateSector")
          .Produces<SectorResponseDto>(200);

        sector.MapPost("/update", async (UpdateSectorRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateSectorCommand(dto));
            return Results.Ok(ApiResponse<SectorResponseDto>.Ok(result, "Sector Updated Successfully"));
        })
        .WithName("UpdateSector")
        .Produces<SectorResponseDto>(200);

        sector.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            bool result = await mediator.Send(new DeleteSectorCommand(id));
            var response = Results.Ok(ApiResponse<bool>.Ok(result, "Sector Deleted Successfully"));
            return Results.Ok(response);
        })
        .WithName("DeleteSector")
        .Produces<bool>(200);

        sector.MapGet("/getByMandalId", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetSectorByMandalIdQuery(id));
            return Results.Ok(ApiResponse<List<SectorByMAndalResponseDto>>.Ok(result));
        })
            .WithName("GetSectorByMandal")
            .Produces<List<SectorByMAndalResponseDto>>(200);

        sector.MapGet("/getAll", async (
            [AsParameters] SectorQueryParams q,
            IMediator mediator, HttpContext httpContext) =>
        {
            string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new GetAllSectorsQuery(q,userId));
            return Results.Ok(ApiResponse<PagedResult<SectorResponseDto>>.Ok(result));
        })
         .WithName("GetAll")
        .RequireAuthorization()
        .Produces<List<SectorResponseDto>>(200);

        sector.MapGet("/getAllSectorReports", async (
            [AsParameters] SectorQueryParams q,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllSectorReportsQuery(q));
            return Results.Ok(ApiResponse<PagedResult<SectorReportDto>>.Ok(result));
        });

        #endregion

        #region Booth
        booth.MapPost("/create", async (BoothRequestDto dto, IMediator mediator, HttpContext http) =>
        {
            string UserId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //var userId = 1;
            //var userName = "Admin";

            var result = await mediator.Send(new CreateBoothCommand(dto, UserId));
            return Results.Ok(ApiResponse<int>.Ok(result, "Booth Created Successfully"));
        })
            .RequireAuthorization(ModulePermission.Booth.ToString())
            .WithName("CreateBooth")
            .Produces<int>(200);

        booth.MapPost("/update", async (updateBoothRequestDto dto, IMediator mediator, HttpContext http) =>
         {
             //var userId = 1;
             //var userName = "Admin";

             bool result = await mediator.Send(new updateBoothCommand(dto));
             return Results.Ok(ApiResponse<bool>.Ok(result, "Booth Updated Successfully"));
         })
            .WithName("UpdateBooth")
            .Produces<int>(200);

        booth.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteBoothCommand(id));
            return Results.Ok("Booth Deleted Successfully");
        });

        booth.MapGet("/getAll", async (
            [AsParameters] BoothQueryParams q,
           IMediator mediator,HttpContext httpContext) =>
        {
            q.UserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new GetAllBoothsQuery(q));
            return Results.Ok(ApiResponse<PagedResult<BoothResponseDto>>.Ok(result));
        });


        #endregion

        #region PannaPramukh

         pannapramukh.MapGet("/getAll", async (
            [AsParameters] PannaPramukhQueryParams q,
            IMediator mediator,
            HttpContext httpContext) =>
         {
            string UserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             var result = await mediator.Send(new GetAllPannaQuery(q));
             return Results.Ok(ApiResponse<PagedResult<PannaPramukhResponseDto>>.Ok(result));
         })
         .WithName("GetAllPannaPramukh")
         .RequireAuthorization(ModulePermission.PannaPramukh.ToString())
         .Produces<ApiResponse<List<PannaPramukhResponseDto>>>(200);

        pannapramukh.MapPost("/create", async (CreatePannaPramukhRequestDto dto, IMediator mediator, HttpContext http) =>
                {
                    string UserId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var result = await mediator.Send(new CreatePannaCommand(dto,UserId));
                    return Results.Ok(ApiResponse<int>.Ok(result, "Panna Pramukh Created Successfully"));
                })
                .WithName("CreatePannaPramukh")
                .RequireAuthorization(ModulePermission.PannaPramukh.ToString())
                .Produces<int>(200);

        pannapramukh.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var res = await mediator.Send(new DeletePannaCommand(id));
            return Results.Ok("Panna Pramukh Deleted Successfully");
        }).WithName("DeletePannaPramukh")
            .RequireAuthorization(ModulePermission.PannaPramukh.ToString())
            .Produces(200);
        pannapramukh.MapPost("/update", async (UpdatePannaPramukhRequestDto dto, IMediator mediator) =>
            {
                int result = await mediator.Send(new UpdatePannaCommand(dto));
                return Results.Ok(ApiResponse<int>.Ok(result, "Panna Pramukh Updated Successfully"));
            })
            .WithName("UpdatePannaPramukh")
             .RequireAuthorization(ModulePermission.PannaPramukh.ToString())
            .Produces<int>(200);
        #endregion

        #region PravasiVoter

        pravasivoter.MapPost("/create", async (CreatePravasiVoterRequestDto dto, IMediator mediator, HttpContext http) =>
        {
            string UserId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new CreatePravasiCommand(dto,UserId));
            return Results.Ok(ApiResponse<int>.Ok(result, "Pravasi Voter Created Successfully"));
        })
                .WithName("CreatePravasiVoter")
                .RequireAuthorization(ModulePermission.PravashiVoter.ToString())
                .Produces<int>(200);

        pravasivoter.MapPost("/update", async (UpdatePravasiVoterRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdatePravasiCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Pravasi Voter Updated Successfully"));
        })
                .WithName("UpdatePravasiVoter")
                .RequireAuthorization(ModulePermission.PravashiVoter.ToString())
                .Produces<int>(200);

        pravasivoter.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeletePravasiCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "Pravasi Voter Deleted Successfully"));
        })
                .WithName("DeletePravasiVoter")
                .RequireAuthorization(ModulePermission.PravashiVoter.ToString())
                .Produces<int>(200);

        pravasivoter.MapGet("/getAll", async (
            [AsParameters] PravasiQueryParams q,
            IMediator mediator, HttpContext httpContext) =>
        {
            q.UserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new GetAllPravasiQuery(q));
            return Results.Ok(ApiResponse<PagedResult<PravasiVoterResponseDto>>.Ok(result));
        }).RequireAuthorization(ModulePermission.PravashiVoter.ToString());

        #endregion

        #region New Voter

        newvoter.MapPost("/create", async (CreateNewVoterRequestDto dto, IMediator mediator, HttpContext http) =>
        {
            string UserId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new CreateNewVoterCommand(dto,UserId));
            return Results.Ok(ApiResponse<int>.Ok(result, "New Voter Created Successfully"));
        }).RequireAuthorization(ModulePermission.NewVoter.ToString())
                .WithName("CreateNewVoter")
                .Produces<int>(200);

        newvoter.MapPost("/update", async (UpdateNewVoterRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateNewVoterCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "New Voter Updated Successfully"));
        })
                .WithName("UpdateNewVoter")
                .Produces<int>(200);

        newvoter.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteNewVoterCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "NewVoter Deleted Successfully"));
        })
                .WithName("DeleteNewVoter")
                .RequireAuthorization(ModulePermission.NewVoter.ToString())
                .Produces<int>(200);
        newvoter.MapGet("/getAll", async (
            [AsParameters] NewVoterQueryParams q,
            IMediator mediator, HttpContext httpContext) =>
        {
             q.UserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new GetAllNewVoterQuery(q));
            return Results.Ok(ApiResponse<PagedResult<NewVoterResponseDto>>.Ok(result));
        }).RequireAuthorization(ModulePermission.NewVoter.ToString());

        #endregion

        #region Booth Voter

        boothvoter.MapPost("/create", async (CreateBoothVoterRequestDto dto, IMediator mediator, HttpContext http) =>
        {
            string UserId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new CreateBoothVoterCommand(dto,UserId));
            return Results.Ok(ApiResponse<int>.Ok(result, "Booth Voter Created Successfully"));
        }).RequireAuthorization(ModulePermission.BoothVoterDescrition.ToString())
                .WithName("CreateBoothVoter")
                .Produces<int>(200);

        boothvoter.MapPost("/update", async (UpdateBoothVoterRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateBoothVoterCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Booth Voter Updated Successfully"));
        }).RequireAuthorization(ModulePermission.BoothVoterDescrition.ToString())
                .WithName("UpdateBoothVoter")
                .Produces<int>(200);

        boothvoter.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteBoothVoterCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "BoothVoter Deleted Successfully"));
        })
                .WithName("DeleteBoothVoter")
                .RequireAuthorization(ModulePermission.BoothVoterDescrition.ToString())
                .Produces<int>(200);
        boothvoter.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllBoothVoterQuery());
            return Results.Ok(ApiResponse<List<BoothVoterResponseDto>>.Ok(result));
        }).RequireAuthorization(ModulePermission.BoothVoterDescrition.ToString());

        #endregion

        #region Caste Voter

        castevoter.MapPost("/create", async (CreateCasteVoterReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateCasteVoterCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Caste Voter Created Successfully"));
        })
                .WithName("CreateCasteVoter")
                .Produces<int>(200);

        castevoter.MapPost("/update", async (UpdateCasteVoterReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateCasteVoterCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Caste Voter Updated Successfully"));
        })
                .WithName("UpdateCasteVoter")
                .Produces<int>(200);

        castevoter.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteCasteVoterCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "Caste Voter Deleted Successfully"));
        })
                .WithName("DeleteCasteVoter")
                .Produces<int>(200);
        castevoter.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllCasteVoterQuery());
            return Results.Ok(ApiResponse<PagedResult<CasteVoterResponseDto>>.Ok(result));
        });

        #endregion

        #region Sahmat Asahmat

        sahmatasahmat.MapPost("/create", async (CreateSahmatAsahmatReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateSahmatAsahmatCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Sahmat/Asahmat Voter Created Successfully"));
        }).RequireAuthorization()
                .WithName("Sahmat/AsahmatNewVoter")
                .Produces<int>(200);

        sahmatasahmat.MapPost("/update", async (UpdateSahmatAsahmatReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateSahmatAsahmatCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Sahmat/Asahmat Updated Successfully"));
        }).RequireAuthorization()
          .WithName("UpdateSahmatAsahmatVoter")
          .Produces<int>(200);

        sahmatasahmat.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteSahmatAsahmatCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "Sahmat/Asahmat Deleted Successfully"));
        }).RequireAuthorization()
                .WithName("DeleteSahmat/Asahmat")
                .Produces<int>(200);

        sahmatasahmat.MapGet("/getAll", async (
            [AsParameters] SahmatAsahmatQueryParams q,
            IMediator mediator) =>
           {
            var result = await mediator.Send(new GetAllSahmatAsahmatQuery(q));
            return Results.Ok(ApiResponse<PagedResult<SahmatAsahmatResponseDto>>.Ok(result));
           }).RequireAuthorization();

        #endregion


        #region Pradhan

        pradhan.MapPost("/create", async (CreatePradhanRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreatePradhanCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Pradhan Created Successfully"));
        }).RequireAuthorization()
                .WithName("CreatePradhan")
                .Produces<int>(200);

        pradhan.MapPost("/update", async (UpdatePradhanRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdatePradhanCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Pradhan Updated Successfully"));
        }).RequireAuthorization()
                .WithName("UpdatePradhan")
                .Produces<int>(200);

        pradhan.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeletePradhanCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "Pradhan Deleted Successfully"));
        }).RequireAuthorization()
          .WithName("DeletePradhan")
          .Produces<int>(200);
        pradhan.MapGet("/getAll", async (
            [AsParameters] PradhanQueryParams q,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllPradhanQuery(q));
            return Results.Ok(ApiResponse<PagedResult<PradhanResponseDto>>.Ok(result));
        });
        #endregion

        #region BoothSamiti

        boothSamiti.MapPost("/create", async (CreateBoothSamitiRequestDto dto, IMediator mediator, HttpContext http) =>
        {
            string UserId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new CreateBoothSamitiCommand(dto, UserId));
            return Results.Ok(ApiResponse<int>.Ok(result, "Booth Samiti Created Successfully"));
        })
        .RequireAuthorization(ModulePermission.BoothSamiti.ToString())
        .WithName("CreateBoothSamiti")
        .Produces<int>(200);


        boothSamiti.MapPost("/update", async (UpdateBoothSamitiRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateBoothSamitiCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Booth Samiti Updated Successfully"));
        })
        .RequireAuthorization(ModulePermission.BoothSamiti.ToString())
        .WithName("UpdateBoothSamiti")
        .Produces<int>(200);


        boothSamiti.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteBoothSamitiCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "Booth Samiti Deleted Successfully"));
        })
         .RequireAuthorization(ModulePermission.BoothSamiti.ToString())
        .WithName("DeleteBoothSamiti")
        .Produces<int>(200);


        boothSamiti.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllBoothSamitiQuery());
            return Results.Ok(ApiResponse<List<BoothSamitiResponseDto>>.Ok(result));
        }).RequireAuthorization(ModulePermission.BoothSamiti.ToString());

        #endregion

        #region influencer

        

        influencer.MapPost("/create", async (CreateInfluencerReqDto dto, IMediator mediator, HttpContext http) =>
        {
            string UserId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new CreateInfluencerCommand(dto, UserId));
            return Results.Ok(ApiResponse<int>.Ok(result, "Influencer Created Successfully"));
        })
                .WithName("CreateInfluencer")
                .Produces<int>(200);

        
        influencer.MapPost("/update", async (UpdateInfluencerReqDto dto, IMediator mediator) =>
        {
            int result = await mediator.Send(new UpdateInfluencerCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Influencer Updated Successfully"));
        })
            .WithName("UpdateInfluencer")
            .Produces<int>(200);

        influencer.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var res = await mediator.Send(new DeleteInfluencerCommand(id));
            return Results.Ok("Influencer Deleted Successfully");
        })
            .WithName("DeleteInfluencer")
            .Produces(200);

        influencer.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllInfluencerQuery());
            return Results.Ok(ApiResponse<List<InfluencerResponseDto>>.Ok(result));
        })
         .WithName("GetAllInfluencer")
         .Produces<ApiResponse<List<InfluencerResponseDto>>>(200);
        #endregion

        #region BoothSamitiDesignation

        boothSamitiDesignation.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllBoothSamitiDesignationQuery());
            return Results.Ok(ApiResponse<List<DesignationDto>>.Ok(result));
        })
.WithName("GetAllBoothSamitiDesignation")
.Produces<ApiResponse<List<DesignationDto>>>(200);
        #endregion

        #region Double Voter

        doublevoter.MapPost("/create", async (CreateDoubleVoterReqDto dto, IMediator mediator, HttpContext http) =>
        {
            string UserId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new CreateDoubleVoterCommand(dto, UserId));
            return Results.Ok(ApiResponse<int>.Ok(result, "New Voter Created Successfully"));
        })
            .RequireAuthorization(ModulePermission.DoubleVoter.ToString())
                .WithName("CreateDoubleVoter")
                .Produces<int>(200);

        doublevoter.MapPost("/update", async (UpdateDoubleVoterRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateDoubleVoterCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Double Voter Updated Successfully"));
        })
                .WithName("UpdateDoubleVoter")
                .Produces<int>(200);

        doublevoter.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteDoubleVoterCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "Double Voter Deleted Successfully"));
        })
                .WithName("DeleteDoubleVoter")
                .Produces<int>(200);

        doublevoter.MapGet("/getAll", async (
            [AsParameters] DoubleVoterQueryParams q, 
            IMediator mediator, HttpContext httpContext) =>
        {
            q.UserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new GetAllDoubleVoterQuery(q));
            return Results.Ok(ApiResponse<PagedResult<DoubleVoterResponseDto>>.Ok(result));
        });

        #endregion

        #region PrabhavShali Vyakti

        prabhavshali.MapPost("/create", async (CreatePrabhavshaliReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreatePrabhavCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Prabhavshali Vyakti Created Successfully"));
        })
               .WithName("CreatePrabhavShali")
               .Produces<int>(200);

        prabhavshali.MapPost("/update", async (UpdatePrabhavshaliReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdatePrabhavCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Prabhavshali Vyakti Updated Successfully"));
        })
                .WithName("UpdatePrabhavshali")
                .Produces<int>(200);

        prabhavshali.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeletePrabhavCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "Prabhavshali Vyakti Deleted Successfully"));
        })
                .WithName("DeletePrabhavshali")
                .Produces<int>(200);

        prabhavshali.MapGet("/getAll", async (
            [AsParameters] PrabhavshaliQueryParams q,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllPrabhavQuery(q));
            return Results.Ok(ApiResponse<PagedResult<PrabhavshaliResponseDto>>.Ok(result));
        });

        prabhavshali.MapGet("/getDesgById", async (int desgId, IMediator mediator) =>
        {
            var result = await mediator.Send(
                new GetAllParabhavshaliByDesignIdQuery
                {
                    DesgId = desgId
                });

            return Results.Ok(ApiResponse<List<PrabhavshaliResponseDesinIdDto>>.Ok(result));
        });

        #endregion

        #region Block

        block.MapPost("/create", async (CreateBlockReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateBlockCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Block Created Successfully"));
        })
                .WithName("CreateBlock")
                .Produces<int>(200);

        block.MapPost("/update", async (UpdateBlockReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateBlockCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Block Updated Successfully"));
        })
                .WithName("UpdateBlock")
                .Produces<int>(200);

        block.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteBlockCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "Block Deleted Successfully"));
        })
                .WithName("DeleteBlock")
                .Produces<int>(200);

        block.MapGet("/getAll", async (
            [AsParameters] BlockQueryParams q,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllBlockQuery(q));
            return Results.Ok(ApiResponse<PagedResult<BlockResponseDto>>.Ok(result));
        });
        block.MapGet("/getAllBlockName", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllBlockNameQuery());
            return Results.Ok(ApiResponse<List<BlockNameResponse>>.Ok(result));
        });

        #endregion

        #region BDC

        bdc.MapPost("/create", async (CreateBDCReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateBDCCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "BDC Created Successfully"));
        })
                .WithName("CreateBDC")
                .Produces<int>(200);

        bdc.MapPost("/update", async (UpdateBDCReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateBDCCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "BDC Updated Successfully"));
        })
                .WithName("UpdateBDC")
                .Produces<int>(200);

        bdc.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteBDCCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "BDC Deleted Successfully"));
        })
                .WithName("DeleteBDC")
                .Produces<int>(200);

        bdc.MapGet("/getAll", async (
            [AsParameters] BDCQueryParams q,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllBDCQuery(q));
            return Results.Ok(ApiResponse<PagedResult<BDCResponseDto>>.Ok(result));
        });

        #endregion

        #region Senior Disabled

        seniordisabled.MapPost("/create", async (CreateSeniorDisabledReqDto dto, IMediator mediator, HttpContext http) =>
        {
            string UserId = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new CreateSeniorDisabledCommand(dto, UserId));
            return Results.Ok(ApiResponse<int>.Ok(result, "SeniorDisabled Created Successfully"));
        })
                .WithName("CreateSeniorDisabled")
                .Produces<int>(200);

        seniordisabled.MapPost("/update", async (UpdateSeniorDisabledReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateSeniorDisabledCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "SeniorDisabled Updated Successfully"));
        })
                .WithName("UpdateSeniorDisabled")
                .Produces<int>(200);

        seniordisabled.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteSeniorDisabledCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "SeniorDisabled Deleted Successfully"));
        })
                .WithName("DeleteSeniorDisabled")
                .Produces<int>(200);

        seniordisabled.MapGet("/getAll", async (
            [AsParameters] SeniorDisabledQueryParams q,
            IMediator mediator,HttpContext httpContext) =>
        {
            q.UserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new GetAllSeniorDisabledQuery(q));
            return Results.Ok(ApiResponse<PagedResult<SeniorDisabledResponseDto>>.Ok(result));
        });


        #endregion
    }
}
