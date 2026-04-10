using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Pannels.Admin.Booth.Command;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Queries;
using VidhanSabha.Application.Pannels.Admin.Mandal.Commands;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Queries;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Queries;
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

        var booth = app.MapGroup("/api/booth")
                     .WithTags("Booth");

        var admin = app.MapGroup("/api/admin")
                        .WithTags("Admin");

        var pannapramukh = app.MapGroup("/api/pannapramukh")
                        .WithTags("PannaPramukh");
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


        #region Booth
        booth.MapPost("/create", async (BoothRequestDto dto, IMediator mediator, HttpContext http) =>
        {
            //var userId = 1;
            //var userName = "Admin";

            var result = await mediator.Send(new CreateBoothCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Booth Created Successfully"));
        })
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

        booth.MapGet("/getAll", async (
         int? mandalId,
           int? sectorId,
           IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllBoothsQuery(mandalId, sectorId));
            return Results.Ok(ApiResponse<List<BoothResponseDto>>.Ok(result));
        });

        booth.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteBoothCommand(id));
            return Results.Ok("Booth Deleted Successfully");
        });

        #endregion

        #region PannaPramukh

        pannapramukh.MapGet("/getAll", async (IMediator mediator) =>
         {
             var result = await mediator.Send(new GetAllPannaQuery());
             return Results.Ok(ApiResponse<List<PannaPramukhResponseDto>>.Ok(result));
         })
         .WithName("GetAllPannaPramukh")
         .Produces<ApiResponse<List<PannaPramukhResponseDto>>>(200);

        pannapramukh.MapPost("/create", async (CreatePannaPramukhRequestDto dto, IMediator mediator) =>
                {
                    var result = await mediator.Send(new CreatePannaCommand(dto));
                    return Results.Ok(ApiResponse<int>.Ok(result, "Panna Pramukh Created Successfully"));
                })
                .WithName("CreatePannaPramukh")
                .Produces<int>(200);
                //pannapramukh.MapPost("/update", async (PannaPramukhRequestDto dto, IMediator mediator) =>
                //    {
                //        var result = await mediator.Send(new UpdatePannaPramukhCommand(dto));
                //        return Results.Ok(ApiResponse<int>.Ok(result, "Panna Pramukh Updated Successfully"));
                //    })
                //    .WithName("UpdatePannaPramukh")
                //    .Produces<int>(200);
        #endregion
    }


    #endregion
}
