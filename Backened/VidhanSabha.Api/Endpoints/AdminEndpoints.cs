using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using VidhanSabha.Api.Responses;
using VidhanSabha.Application.Pannels.Admin.BDC.Command;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;
using VidhanSabha.Application.Pannels.Admin.BDC.Queries;
using VidhanSabha.Application.Pannels.Admin.Block.Command;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.Block.Queries;
using VidhanSabha.Application.Pannels.Admin.Booth.Command;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Queries;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Command;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Queries;
using VidhanSabha.Application.Pannels.Admin.Mandal.Commands;
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
        var doublevoter = app.MapGroup("/api/doublevoter")
                        .WithTags("DoubleVoter");
        var prabhavshali = app.MapGroup("/api/prabhavshali")
                        .WithTags("PrabhavShali");
        var block = app.MapGroup("/api/block")
                        .WithTags("Block");
        var bdc = app.MapGroup("/api/bdc")
                        .WithTags("BDC");
        var seniordisabled = app.MapGroup("/api/seniordisabled")
                        .WithTags("SeniorDisabled");

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
        #endregion

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

        pannapramukh.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var res = await mediator.Send(new DeletePannaCommand(id));
            return Results.Ok( "Panna Pramukh Deleted Successfully");
        })
            .WithName("DeletePannaPramukh")
            .Produces(200);
        pannapramukh.MapPost("/update", async (UpdatePannaPramukhRequestDto dto, IMediator mediator) =>
            {
                int result = await mediator.Send(new UpdatePannaCommand(dto));
                return Results.Ok(ApiResponse<int>.Ok(result, "Panna Pramukh Updated Successfully"));
            })
            .WithName("UpdatePannaPramukh")
            .Produces<int>(200);
        #endregion

        #region PravasiVoter

        pravasivoter.MapPost("/create", async (CreatePravasiVoterRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreatePravasiCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Pravasi Voter Created Successfully"));
        })
                .WithName("CreatePravasiVoter")
                .Produces<int>(200);

        pravasivoter.MapPost("/update", async (UpdatePravasiVoterRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdatePravasiCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Pravasi Voter Updated Successfully"));
        })
                .WithName("UpdatePravasiVoter")
                .Produces<int>(200);

        pravasivoter.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeletePravasiCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "Pravasi Voter Deleted Successfully"));
        })
                .WithName("DeletePravasiVoter")
                .Produces<int>(200);

        pravasivoter.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllPravasiQuery());
            return Results.Ok(ApiResponse<List<PravasiVoterResponseDto>>.Ok(result));
        });

        #endregion

        #region New Voter

        newvoter.MapPost("/create", async (CreateNewVoterRequestDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateNewVoterCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "New Voter Created Successfully"));
        })
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
                .Produces<int>(200);
        newvoter.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllNewVoterQuery());
            return Results.Ok(ApiResponse<List<NewVoterResponseDto>>.Ok(result));
        });

        #endregion

        #region Sahmat Asahmat

        sahmatasahmat.MapPost("/create", async (CreateSahmatAsahmatReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateSahmatAsahmatCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Sahmat/Asahmat Voter Created Successfully"));
        })
                .WithName("Sahmat/AsahmatNewVoter")
                .Produces<int>(200);

        sahmatasahmat.MapPost("/update", async (UpdateSahmatAsahmatReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateSahmatAsahmatCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "Sahmat/Asahmat Updated Successfully"));
        })
                .WithName("UpdateSahmatAsahmatVoter")
                .Produces<int>(200);

        sahmatasahmat.MapPost("/delete", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteSahmatAsahmatCommand(id));
            return Results.Ok(ApiResponse<int>.Ok(result, "Sahmat/Asahmat Deleted Successfully"));
        })
                .WithName("DeleteSahmat/Asahmat")
                .Produces<int>(200);

        sahmatasahmat.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllSahmatAsahmatQuery());
            return Results.Ok(ApiResponse<List<SahmatAsahmatResponseDto>>.Ok(result));
        });

        #endregion

        #region Double Voter

        doublevoter.MapPost("/create", async (CreateDoubleVoterReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateDoubleVoterCommand(dto));
            return Results.Ok(ApiResponse<int>.Ok(result, "New Voter Created Successfully"));
        })
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

        doublevoter.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllDoubleVoterQuery());
            return Results.Ok(ApiResponse<List<DoubleVoterResponseDto>>.Ok(result));
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

        prabhavshali.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllPrabhavQuery());
            return Results.Ok(ApiResponse<List<PrabhavshaliResponseDto>>.Ok(result));
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

        block.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllBlockQuery());
            return Results.Ok(ApiResponse<List<BlockResponseDto>>.Ok(result));
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

        bdc.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllBDCQuery());
            return Results.Ok(ApiResponse<List<BDCResponseDto>>.Ok(result));
        });

        #endregion

        #region Senior Disabled

        seniordisabled.MapPost("/create", async (CreateSeniorDisabledReqDto dto, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateSeniorDisabledCommand(dto));
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

        seniordisabled.MapGet("/getAll", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAllSeniorDisabledQuery());
            return Results.Ok(ApiResponse<List<SeniorDisabledResponseDto>>.Ok(result));
        });

        #endregion
    }



}
