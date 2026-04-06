using MediatR;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Common.Category.DTOs;
using VidhanSabha.Application.Common.Category.Queries;
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
                var result = await mediator.Send(new getall());
               return Results.Ok(ApiResponse<List<CategoryResponseDto>>.Ok(result));
            })
        .WithName("GetAllCategories")
        .Produces<ApiResponse<List<CategoryResponseDto>>>(200);
        }
    }
}
