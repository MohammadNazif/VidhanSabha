using MediatR;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Common.District.Queries;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.Commands;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.DTOs;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.Queries;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Query;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Command;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Query;
using VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Command;
using VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Query;
using static VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Dtos.DistrictWiseCount;
using static VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Dtos.VidhanshabhaStateWiseCount;

namespace VidhanSabha.Api.Endpoints
{
    public static class SuperAdminEndpoints
    {
            public static void MapSuperAdminEndpoints(this WebApplication app)
            {
                var designation = app.MapGroup("/api/designation")
                                    .WithTags("Designation");
                var vidhansabhacount = app.MapGroup("/api/vidhansabhacount")
                                    .WithTags("Vidhansabhacount");

            var stateprabhaari = app.MapGroup("/api/stateprabhari")
                                    .WithTags("Stateprabhari");
            designation.MapPost
               ("/create", async (
               CreateDesignationDto request,
               IMediator mediator) =>
               {
                    var data = await mediator.Send(new CreateDesignationCommand(request));
                   return Results.Ok(ApiResponse<int>.Ok(data,"Designation Created Successfully"));

               }).WithName("CreateDesignation")
               .Produces(StatusCodes.Status200OK);

            designation.MapGet("/getAll", async (string userId,IMediator mediator) =>
            {
                var data = await mediator.Send(new getAllDesignationQuery(userId));
                return Results.Ok(ApiResponse<IReadOnlyList<DesignationResponseDto>>.Ok(data, "Designation fetched Successfully"));
            }).WithName("GetAllDesignation")
            .Produces(StatusCodes.Status200OK);

            designation.MapPost("/update", async (
               UpdateDesignationDto request,
               IMediator mediator) =>
            {
                var data = await mediator.Send(new UpdateDesignationCommand(request));
                return Results.Ok(ApiResponse<int>.Ok(data, "Designation Updated Successfully"));
            }).WithName("UpdateDesignation");

            designation.MapPost("/delete", async (int id, IMediator mediator) =>
            {
                var data = await mediator.Send(new DeleteDesignationCommand(id));
                return Results.Ok(ApiResponse<int>.Ok(data, "Designation Deleted Successfully"));
            }).WithName("DeleteDesignation")
             .Produces(StatusCodes.Status200OK);

            vidhansabhacount.MapPost("/create", async (IMediator mediator, VidhansabhaRequestDto requestDto) =>
            {
                var data = await mediator.Send(new CreateVidhansabhaCountCommand(requestDto));
               return Results.Ok(ApiResponse<int>.Ok(data, "VidhanSabha Count inserted SuccessFully"));
            });

            vidhansabhacount.MapGet("/getAll", async (string? userId,IMediator mediator) =>
            {
                var data = await mediator.Send(new getAllvidhanSabhaCountQuery(userId));
                return Results.Ok(ApiResponse<IReadOnlyList<VidhansabhaResponseDto>>.Ok(data, "Vidhansabha Count Fetched Successfully"));
            }).WithName("GetAllVidhanSabhaCount")
            .Produces<ApiResponse<IReadOnlyList<VidhansabhaResponseDto>>>(200);

            stateprabhaari.MapPost("/create", async (IMediator mediator, CreatePrabhariRequestDto requestDto) =>
            {
                var data = await mediator.Send(new CreatePrabhariCommand(requestDto));
                return Results.Ok(ApiResponse<int>.Ok(data, "State Prabhari inserted SuccessFully"));
             });

            stateprabhaari.MapGet("/getAll",async (IMediator mediator) =>
            {
                var data = await mediator.Send(new getAllStatePrabhariQuery());
                return Results.Ok(ApiResponse<IReadOnlyList<StatePrabhariResponseDto>>.Ok(data, "State Prabhari Fetched Successfully"));
            }).WithName("GetAllStatePrabhari")
            .Produces<ApiResponse<IReadOnlyList<StatePrabhariResponseDto>>>(200);

            stateprabhaari.MapPost("/update", async (IMediator mediator, UpdatePrabhariRequestDto requestDto) =>
            {
                var data = await mediator.Send(new UpdatePrabhariCommand(requestDto));
                return Results.Ok(ApiResponse<int>.Ok(data, "State Prabhari Updated Successfully"));
            }).WithName("updateStatePrabhari")
             .Produces(200);

            //vidhansabhacount.MapPost("/create/", async (int id,string userId, IMediator mediator) =>
            //{
            //    var data = await mediator.Send(new DeletePrabhariCommand(id,userId));
            //    return Results.Ok(ApiResponse<int>.Ok(data, " State Prabhari deleted successfully"));
            //}).Produces(200);

            vidhansabhacount.MapPost("districtwise/create", async (IMediator mediator, VidhansabhaDistrictRequestDto request) =>
            {
                var data = await mediator.Send(new CreateDistrictWiseCount(request));
                return Results.Ok(ApiResponse<int>.Ok(data, " VidhanSabha Count inserted successfully"));
            }).WithName("CountistrictWise")
            .Produces(200);

            vidhansabhacount.MapGet("districtwise/getAll", async (IMediator mediator, string userId) =>
            {
                var data = await mediator.Send(new getAllDitrictWiseCountQuery(userId));
                return Results.Ok(ApiResponse<IReadOnlyList<VidhansabhaDistrictResponseDto>>.Ok(data, "Count fetched Successfully"));
            }).WithName("getallDistrictwisecount")
            .Produces(200);
        }
    }
}
