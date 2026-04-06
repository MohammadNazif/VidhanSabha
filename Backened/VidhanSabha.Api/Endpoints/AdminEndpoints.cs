using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Pannels.Admin.Mandal.Commands;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.Queries;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this WebApplication app)
    {
        var mandal = app.MapGroup("/api/mandal")
                        .WithTags("Mandal");

        var admin = app.MapGroup("/api/admin")
                        .WithTags("Admin");

        // ── GET ALL ──────────────────────────────────────────
        mandal.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllMandalsQuery());

            return Results.Ok(ApiResponse<List<MandalResponseDto>>.Ok(result));
        })
        .WithName("GetAllMandals")
        .Produces<ApiResponse<List<MandalResponseDto>>>(200);

        mandal.MapPost("/create", async (
            CreateMandalRequestDto request,
            IMediator mediator) =>
        {
            var result = await mediator.Send(
                new CreateMandalCommand(request.VidhanId, request.Name));

            return Results.Created($"/api/mandal/{result.Id}",
                ApiResponse<MandalResponseDto>.Ok(result));
        })
        .WithName("CreateMandal")
        .Produces<ApiResponse<MandalResponseDto>>(201)
        .Produces(400);

        // Moved admin.MapGet inside the MapAdminEndpoints method body
        //admin.MapGet("/", async (IMediator mediator) =>
        //{
        //    var result = await mediator.Send(new GetAllMandalsQuery());

        //    return Results.Ok(ApiResponse<List<MandalResponseDto>>.Ok(result));
        //})
        //.WithName("GetAllMandals")
        //.Produces<ApiResponse<List<MandalResponseDto>>>(200);
    }
}