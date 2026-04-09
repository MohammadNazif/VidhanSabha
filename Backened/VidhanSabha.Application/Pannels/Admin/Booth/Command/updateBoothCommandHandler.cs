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
    public class updateBoothCommandHandler : IRequestHandler<updateBoothCommand, bool>
    {
        private IBoothRepository _repo;

        public updateBoothCommandHandler(IBoothRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(updateBoothCommand request, CancellationToken cancellationToken)
        {
            // ✅ Step 1 — Validate request
            var dto = request.Dto;

            if (dto.IsBoothSanyojak && dto.Sanyojak == null)
                throw new Exception("Sanyojak details required when IsBoothSanyojak is true");

            // ✅ Step 2 — Load existing booth WITH children (critical!)
            var booth = await _repo.GetByIdAsync(dto.id, cancellationToken);

            if (booth == null)
                throw new Exception($"Booth with Id {dto.id} not found");

            // ✅ Step 3 — Build villages list
            var villages = dto.Villages
                .Select(v => Tbl_BoothVillage.Create(v.VillageId, v.HasAnshik))
                .ToList();

            // ✅ Step 4 — Build sanyojak only if needed
            // Note: Pass null if !IsBoothSanyojak — domain entity handles the rest
            Tbl_BoothSanyojak? sanyojak = null;
            if (dto.IsBoothSanyojak && dto.Sanyojak != null)
            {
                sanyojak = Tbl_BoothSanyojak.Create(  // ✅ Use Create (no .update() static method)
                    dto.Sanyojak.InchargeName,
                    dto.Sanyojak.Age,
                    dto.Sanyojak.FatherName,
                    dto.Sanyojak.CategoryId,
                    dto.Sanyojak.CastId,
                    dto.Sanyojak.EducationLevel,
                    dto.Sanyojak.PhoneNumber,
                    dto.Sanyojak.Address
                );
            }

            // ✅ Step 5 — Call domain Update (instance method on loaded booth)
            // Domain handles:
            // - isBoothSanyojak=false → sets Sanyojak=null (EF deletes via cascade)
            // - isBoothSanyojak=true, existing sanyojak → calls UpdateProfile()
            // - isBoothSanyojak=true, no existing sanyojak → assigns new one
            // - villages → clears old, adds new (EF deletes+inserts)
            booth.Update(
                dto.MandalId,
                dto.SectorId,
                dto.BoothNumber,
                dto.PollingStationName,
                dto.PollingStationLocation,
                dto.IsBoothSanyojak,
                villages,
                sanyojak
            );

            // ✅ Step 6 — Persist
            await _repo.UpdateAsync(booth, cancellationToken);

            return true;
        }

    }
    }
