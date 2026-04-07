using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Pannels.Admin.Mandal.Commands;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Queries;
using VidhanSabha.Application.Pannels.Admin.Sector.Commands;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Queries;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this WebApplication app)
    {
        var mandal = app.MapGroup("/api/mandal")
                        .WithTags("Mandal");

        var sector = app.MapGroup("/api/sector")
                       .WithTags("Sector");

        var admin = app.MapGroup("/api/admin")
                        .WithTags("Admin");
        #region Mandal
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

        mandal.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteMandalCommand(id));

                return Results.Ok(ApiResponse<int>.Ok(result,"Mandal Deleted Succesfully"));
        })
         .WithName("DeleteMandal")
         .Produces<int>(200);

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

        sector.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllSectorsQuery());
            return Results.Ok(ApiResponse<List<SectorResponseDto>>.Ok(result));
        }).WithName("GetAll")
        .Produces<List<SectorResponseDto>>(200);


        //sector.MapPost("/delete", async (int id, IMediator mediator) =>
        //{
        //    var result = await mediator.Send(new DeleteSectorCommand(id));
        //    return Results.Ok(ApiResponse<int>.Ok(result, "Sector Deleted Successfully"));
        //})
        //.WithName("DeleteSector")
        //.Produces<int>(200);


        //sector.MapGet("/getbyid", async (int id, IMediator mediator) =>
        //{
        //    var result = await mediator.Send(new GetSectorByIdQuery(id));
        //    return Results.Ok(ApiResponse<SectorResponseDto>.Ok(result, "Sector Fetched Successfully"));
        //})
        //.WithName("GetSectorById")
        //.Produces<SectorResponseDto>(200);


        //sector.MapGet("/getall", async (IMediator mediator) =>
        //{
        //    var result = await mediator.Send(new GetAllSectorsQuery());
        //    return Results.Ok(ApiResponse<List<SectorResponseDto>>.Ok(result, "Sectors Fetched Successfully"));
        //})
        //.WithName("GetAllSectors")
        //.Produces<List<SectorResponseDto>>(200);
    }


    #endregion
}
