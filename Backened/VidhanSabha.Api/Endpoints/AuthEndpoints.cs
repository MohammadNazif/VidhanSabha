// Endpoints/AuthEndpoints.cs
using MediatR;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Common.MemberModulePermission.Command;
using VidhanSabha.Application.Common.MemberModulePermission.Dtos;
using VidhanSabha.Application.Common.MemberModulePermission.Query;
using VidhanSabha.Application.Pannels.Auth.Commands.Login;
using VidhanSabha.Application.Pannels.Auth.DTOs;
using VidhanSabha.Application.Pannels.Auth.Queries.GetMobileNumber;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var auth = app.MapGroup("/api/auth")
                      .WithTags("Auth");

        var permissions = app.MapGroup("/api/permission")
              .WithTags("Permission");


        // ── LOGIN ────────────────────────────────────────────
        auth.MapPost("/login", async (
            LoginRequestDto request,
            IMediator mediator) =>
        {
            var result = await mediator.Send(
                new LoginCommand(request.MobileNumber, request.Password));

            return result is null
                ? Results.Unauthorized()
                : Results.Ok(ApiResponse<LoginResponseDto>.Ok(result));
        })
        .WithName("Login")
        .Produces<ApiResponse<LoginResponseDto>>(200)
        .Produces<ApiResponse<LoginResponseDto>>(401);

        // ── GET USER ─────────────────────────────────────────
        auth.MapGet("/user/{mobile}", async (
            string mobile,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUserByMobileQuery(mobile));

            return result is null
                ? Results.NotFound(ApiResponse<LoginRequestDto>.NotFound("User not found."))
                : Results.Ok(ApiResponse<LoginResponseDto>.Ok(result));
        })
        .WithName("GetUserByMobile")
        .Produces<ApiResponse<LoginResponseDto>>(200)
        .Produces<ApiResponse<LoginRequestDto>>(404);

        permissions.MapPost("/create", async (IMediator mediator, List<MemberModulePermissionDto> request) =>
        {
            var data = await mediator.Send(new CreateMemberModulePermissionCommand(request));
            return Results.Ok(ApiResponse<int>.Ok(data, "Permession Granted Successfully"));
        }).WithName("accessPermission")
        .Produces(200);

        permissions.MapPost("/getbyuserid", async (IMediator mediator,string UserId) =>
        {
            var data = await mediator.Send(new getAllPermissionbyUseridQuery(UserId));
            return Results.Ok(ApiResponse<IReadOnlyList<MemberModulePermissionResDto>>.Ok(data, "Permession fetched Successfully"));
        }).WithName("getPermission")
      .Produces(200);
    }
}