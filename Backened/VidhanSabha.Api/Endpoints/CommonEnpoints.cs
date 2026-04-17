using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Common.Booth.Dtos;
using VidhanSabha.Application.Common.Booth.Queries;
using VidhanSabha.Application.Common.Cast.DTOs;
using VidhanSabha.Application.Common.Cast.Queries;
using VidhanSabha.Application.Common.Category.DTOs;
using VidhanSabha.Application.Common.Category.Queries;
using VidhanSabha.Application.Common.DesignatinType.Dto;
using VidhanSabha.Application.Common.DesignatinType.Query;
using VidhanSabha.Application.Common.District.DTOs;
using VidhanSabha.Application.Common.District.Queries;
using VidhanSabha.Application.Common.Occupation.DTOs;
using VidhanSabha.Application.Common.Occupation.Queries;
using VidhanSabha.Application.Common.Party.DTOs;
using VidhanSabha.Application.Common.Party.Queries;
using VidhanSabha.Application.Common.SahmatAsahmatType.DTOs;
using VidhanSabha.Application.Common.SahmatAsahmatType.Queries;
using VidhanSabha.Application.Common.State.Dto;
using VidhanSabha.Application.Common.State.Query;
using VidhanSabha.Application.Common.Village.DTOs;
using VidhanSabha.Application.Common.Village.Queries;
using VidhanSabha.Application.Pannels.Auth.DTOs;

namespace VidhanSabha.Api.Endpoints
{
    public static class CommonEnpoints
    {

        public static void MapCommonEndpoints(this WebApplication app)
        {
            var common = app.MapGroup("/api/common")
                          .WithTags("Common");

            // ── LOGIN ────────────────────────────────────────────
            common.MapGet("/category", async (IMediator mediator) =>
              {
                  var result = await mediator.Send(new GetallCatgeory());
                  return Results.Ok(ApiResponse<List<CategoryResponseDto>>.Ok(result));
              })
            .WithName("GetAllCategories")
            .Produces<ApiResponse<List<CategoryResponseDto>>>(200);

            common.MapGet("/cast", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new getAllCastQuery(id));
                return Results.Ok(ApiResponse<List<CastResponseDto>>.Ok(result));

            })
             .WithName("GetCast")
             .Produces<ApiResponse<List<CastResponseDto>>>(200);

            common.MapGet("/village", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetallVillageByMandalId(id));
                return Results.Ok(ApiResponse<List<VillageResponseDto>>.Ok(result));
            })
             .WithName("GetAllVillages")
             .Produces<List<VillageResponseDto>>(200);

            common.MapGet("/boothNumber", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllBoothNumbersQuery());
                return Results.Ok(ApiResponse<List<BoothNumberDto>>.Ok(result));
            })
             .WithName("GetBoothNumbers")
             .Produces<List<BoothNumberDto>>(200);

            common.MapGet("/villagesByBoothId", async (int boothId, IMediator mediator) =>
               {
                   var result = await mediator.Send(new GetallVillageByBoothId(boothId));
                   return Results.Ok(ApiResponse<List<VillageByBoothResponseDto>>.Ok(result));
               })
                .WithName("GetVillagesByBoothId")
                .Produces<List<VillageByBoothResponseDto>>(200);

            common.MapGet("/designationType", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new getdesignationTypeQuery());
                return Results.Ok(ApiResponse<List<DesignationTypeResponseDto>>.Ok(result));
            })
             .WithName("GetDesignationTypes")
             .Produces<List<DesignationTypeResponseDto>>(200);

            common.MapGet("/getstates", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new getAllStateQuery());
                return Results.Ok(ApiResponse<IReadOnlyList<StateResponseDto>>.Ok(result));
             })
             .WithName("getstates")
             .Produces<List<StateResponseDto>>(200);

            common.MapGet("/getdistrict", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetAllDistrictQuery(id));
                return Results.Ok(ApiResponse<List<DistrictResponseDto>>.Ok(result));

            })
             .WithName("GetDistrict")
             .Produces<ApiResponse<List<DistrictResponseDto>>>(200);

            common.MapGet("/getoccupation", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllOccupationQuery());
                return Results.Ok(ApiResponse<IReadOnlyList<OccupationResponseDto>>.Ok(result));
            })
                .WithName("getoccupation")
                .Produces<List<OccupationResponseDto>>(200);

            common.MapGet("/getparty", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllPartyQuery());
                return Results.Ok(ApiResponse<List<PartyResponseDto>>.Ok(result));
            })
                .WithName("getparty")
                .Produces<List<PartyResponseDto>>(200);

            common.MapGet("/getsahmattype", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllSahmatTypeQuery());
                return Results.Ok(ApiResponse<List<SahmatTypeResponseDto>>.Ok(result));
            })
                .WithName("getsahmattype")
                .Produces<List<SahmatTypeResponseDto>>(200);
        }
    }
}
