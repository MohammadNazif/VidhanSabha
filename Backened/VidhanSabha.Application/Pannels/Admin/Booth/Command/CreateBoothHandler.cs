using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Command
{
    public class CreateBoothHandler : IRequestHandler<CreateBoothCommand, int>
    {
        private readonly IBoothRepository _repo;
        //private readonly IFileStorageService _files;

        public CreateBoothHandler(IBoothRepository repo)
        {
            _repo = repo;
            //_files = files;
        }

        public async Task<int> Handle(CreateBoothCommand request, CancellationToken ct)
        {
            //if (await _repo.BoothNumberExistsAsync(cmd.MandalId, cmd.BoothNumber, null, ct))
            //    return Result.Failure<int>($"Booth {cmd.BoothNumber} already exists in this Mandal.");

            // Build villages
            var cmd  = request.Dto;
            var villages = cmd.Villages
                .Select(v => Tbl_BoothVillage.Create(v.VillageId, v.HasAnshik))
                .ToList();

            // Build sanyojak — null if IsBoothSanyojak = false
            Tbl_BoothSanyojak? sanyojak = null;
            if (cmd.IsBoothSanyojak && cmd.Sanyojak is not null)
            {
                //string? imgPath = cmd.Sanyojak.ProfileImage is not null
                //    ? await _files.SaveAsync(cmd.Sanyojak.ProfileImage, "sanyojak", ct)
                //    : null;

                sanyojak = Tbl_BoothSanyojak.Create(
                    cmd.Sanyojak.InchargeName, cmd.Sanyojak.Age,
                    cmd.Sanyojak.FatherName, cmd.Sanyojak.CategoryId,
                    cmd.Sanyojak.CastId, cmd.Sanyojak.EducationLevel,
                    cmd.Sanyojak.PhoneNumber, cmd.Sanyojak.Address);
            }

            var booth = Tbl_Booth.Create(
                cmd.MandalId, cmd.SectorId, cmd.BoothNumber,
                cmd.PollingStationName, cmd.PollingStationLocation,
                cmd.IsBoothSanyojak, villages, sanyojak);

                await _repo.AddAsync(booth, ct);
             

            return booth.Id;
        }
    }
}

