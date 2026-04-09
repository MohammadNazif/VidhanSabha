using MediatR;
using Microsoft.AspNetCore.Mvc;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Common.Cast.DTOs;
using VidhanSabha.Application.Common.Cast.Queries;
using VidhanSabha.Application.Common.Category.DTOs;
using VidhanSabha.Application.Common.Category.Queries;
using VidhanSabha.Application.Common.Village.DTOs;
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
                var result = await mediator.Send(new GetallVillage(id));
                return Results.Ok(ApiResponse<List<VillageResponseDto>>.Ok(result));
            })
             .WithName("GetAllVillages")
             .Produces<List<VillageResponseDto>>(200);
        }
    }
}
