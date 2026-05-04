using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Common.District.Queries;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Dashboard.Dtos;
using VidhanSabha.Application.Pannels.Admin.Dashboard.Query;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Queries;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Command;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Query;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.Commands;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.DTOs;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.Queries;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Commands;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.DTOs;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Queries;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Query;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Command;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Query;
using VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Command;
using VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Query;
using VidhanSabha.Domain.Enums;
using VidhanSabha.Infrastructure.Repositories.Admin;
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

            var statemembers = app.MapGroup("/api/statemembers")
                                    .WithTags("StateMembers");

            designation.MapPost
               ("/create", async (
               CreateDesignationDto request,
               IMediator mediator) =>
               {
                    var data = await mediator.Send(new CreateDesignationCommand(request));
                   return Results.Ok(ApiResponse<int>.Ok(data,"Designation Created Successfully"));

               }).WithName("CreateDesignation")
               .Produces(StatusCodes.Status200OK);

            designation.MapGet("/getAll", async (IMediator mediator,HttpContext httpContext) =>
            {
                string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
                var data = await mediator.Send(new getAllDesignationQuery(userId));
                return Results.Ok(ApiResponse<IReadOnlyList<DesignationResponseDto>>.Ok(data, "Designation fetched Successfully"));
            }).WithName("GetAllDesignation")
            .RequireAuthorization()
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

            stateprabhaari.MapGet("/vidhansabha/getAll", async (IMediator mediator,HttpContext httpContext,[AsParameters] vidhansabhaparams q,int? districtId) =>
            {
                q.UserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var data = await mediator.Send(new getAllVidhanSabhaQuery(q,districtId));
                return Results.Ok(ApiResponse<IReadOnlyList<VidhanSabhaSatewiseResponseDto>>.Ok(data, "VidhanSabha Fetched Successfully"));
            }).WithName("GetAllstatewiseVidhansabha")
            .RequireAuthorization()
            .Produces<ApiResponse<IReadOnlyList<VidhanSabhaSatewiseResponseDto>>>(200);

            stateprabhaari.MapGet("/getAll", async (IMediator mediator) =>
            {
                var data = await mediator.Send(new getAllStatePrabhariQuery());
                return Results.Ok(ApiResponse<IReadOnlyList<StatePrabhariResponseDto>>.Ok(data, "State Prabhari Fetched Successfully"));
            }).WithName("GetAllStatePrabhari")
     .Produces<ApiResponse<IReadOnlyList<StatePrabhariResponseDto>>>(200);

            stateprabhaari.MapGet("/dashboardcount", async (IMediator mediator,HttpContext httpContext) =>
            {
                string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var data = await mediator.Send(new GetStateDashboardCounts(userId));
                return Results.Ok(ApiResponse<StateDashboardCountsDto>.Ok(data, "State Counts Fetched Successfully"));
            }).WithName("Statedashboardcount")
            .RequireAuthorization()
            .Produces<ApiResponse<StateDashboardCountsDto>>(200);

            stateprabhaari.MapGet("vidhansabhaPrabhari/getAll", async (IMediator mediator,int stateId,HttpContext httpContext) =>
            {
                string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var data = await mediator.Send(new getAllVidhanSabhaPrabhariQuery(stateId,userId));
                return Results.Ok(ApiResponse<IReadOnlyList<StatePrabhariResponseDto>>.Ok(data, "VidhanSabha Prabhari Fetched Successfully"));
            }).WithName("GetAllVidhanSabhaPrabhari")
           .Produces<ApiResponse<IReadOnlyList<StatePrabhariResponseDto>>>(200);

            stateprabhaari.MapPost("/update", async (IMediator mediator, UpdatePrabhariRequestDto requestDto) =>
            {
                var data = await mediator.Send(new UpdatePrabhariCommand(requestDto));
                return Results.Ok(ApiResponse<int>.Ok(data, "State Prabhari Updated Successfully"));
            }).WithName("updateStatePrabhari")
             .Produces(200);



            stateprabhaari.MapPost("/vidhansabha/create", async (IMediator mediator, CreateVidhanSabhaRequestDto requestDto,HttpContext httpContext) =>
            {
                string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var data = await mediator.Send(new CreateVidhanSabhaCommand(requestDto,userId));
                return Results.Ok(ApiResponse<int>.Ok(data, "Vidhansabha Created Successfully"));
            }).WithName("VidhanSabhaCreated")
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
            }).WithName("CountdistrictWise")
            .Produces(200);

            vidhansabhacount.MapPost("districtwise/update", async (IMediator mediator, UpdateVidhansabhaDistrictReqDto request) =>
            {
                var data = await mediator.Send(new UpdateDistrictWiseCount(request));
                return Results.Ok(ApiResponse<int>.Ok(data, " VidhanSabha Count updated successfully"));
            }).WithName("updateCountdistrictWise")
            .Produces(200);

            vidhansabhacount.MapGet("districtwise/getAll", async (IMediator mediator, string userId) =>
            {
                var data = await mediator.Send(new getAllDitrictWiseCountQuery(userId));
                return Results.Ok(ApiResponse<IReadOnlyList<VidhansabhaDistrictResponseDto>>.Ok(data, "Count fetched Successfully"));
            }).WithName("getallDistrictwisecount")
            .Produces(200);

            #region State Members

            statemembers.MapPost("/create", async ([FromForm] CreateStateMembersReqDto dto, IMediator mediator,HttpContext httpContext) =>
            {
                string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await mediator.Send(new CreateStateMembersCommand(dto,userId));
                return Results.Ok(ApiResponse<int>.Ok(result, "State Member Created Successfully"));
            }).RequireAuthorization()
                .WithName("CreateStateMember")
                .DisableAntiforgery()
                .Produces<int>(200);

            statemembers.MapPost("/update", async ([FromForm] UpdateStateMembersReqDto dto, IMediator mediator) =>
            {
                int result = await mediator.Send(new UpdateStateMembersCommand(dto));
                return Results.Ok(ApiResponse<int>.Ok(result, "State Member Updated Successfully"));
            })
                .WithName("UpdateStateMember")
                 .DisableAntiforgery()
                .Produces<int>(200);

            statemembers.MapPost("/delete", async (int id, IMediator mediator) =>
            {
                var res = await mediator.Send(new DeleteStateMembersCommand(id));
                return Results.Ok("State Member Deleted Successfully");
            }).WithName("DeleteStateMember")
                .Produces(200);

            statemembers.MapGet("/getAll", async (
                [AsParameters] StateMembersQueryParams q,int? samitiType,
                IMediator mediator
               ) =>
            {
                var result = await mediator.Send(new GetAllStateMembersQuery(q,samitiType));
                return Results.Ok(ApiResponse<PagedResult<StateMembersResponseDto>>.Ok(result));
            })
             .WithName("GetAllStateMember")
             .Produces<ApiResponse<List<StateMembersResponseDto>>>(200);

            #endregion
        }
    }
}
