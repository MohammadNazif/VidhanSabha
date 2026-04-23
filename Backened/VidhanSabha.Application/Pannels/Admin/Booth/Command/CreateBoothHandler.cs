using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Command
{
    public class CreateBoothHandler : IRequestHandler<CreateBoothCommand, int>
    {
        private readonly IBoothRepository _repo;
        private readonly CredentialManagerFunc _credentialManager;
        private readonly IUnitOfWork _uow;

        public CreateBoothHandler(
            IBoothRepository repo,
            CredentialManagerFunc credentialManager,
            IUnitOfWork uow)
        {
            _repo = repo;
            _credentialManager = credentialManager;
            _uow = uow;
        }

        public async Task<int> Handle(CreateBoothCommand request, CancellationToken ct)
        {
            var cmd = request.Dto;

            // Generate userId the same way as StatePrabhari
            var userId = $"USR_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

            await _uow.BeginTransactionAsync();
            try
            {
                // ── Step 1: Build villages (no DB call, just domain objects) ──────
                var villages = cmd.Villages
                    .Select(v => Tbl_BoothVillage.Create(v.VillageId, v.HasAnshik))
                    .ToList();

                // ── Step 2: Handle Sanyojak credential + entity ───────────────────
                Tbl_BoothSanyojak? sanyojak = null;

                if (cmd.IsBoothSanyojak && cmd.Sanyojak is not null)
                {
                    // Step 2a — Insert credential (tbl_login) first, same as Prabhari
                    await _credentialManager.InsertCredentialAsync(
                        userId: userId,
                        mobile: cmd.Sanyojak.PhoneNumber,
                        email: "",
                        role: "BoothSanyojak"
                    );

                    // Step 2b — Build Sanyojak domain entity with userId
                    sanyojak = Tbl_BoothSanyojak.Create(
                        userId,
                        cmd.Sanyojak.InchargeName,
                        cmd.Sanyojak.Age,
                        cmd.Sanyojak.FatherName,
                        cmd.Sanyojak.CategoryId,
                        cmd.Sanyojak.CastId,
                        cmd.Sanyojak.EducationLevel,
                        cmd.Sanyojak.PhoneNumber,
                        cmd.Sanyojak.Address
                    );
                }

                // ── Step 3: Build Booth aggregate and persist ─────────────────────
                var booth = Tbl_Booth.Create(
                    cmd.MandalId,
                    cmd.SectorId,
                    cmd.BoothNumber,
                    cmd.PollingStationName,
                    cmd.PollingStationLocation,
                    cmd.IsBoothSanyojak,
                    villages,  
                    sanyojak);

                 var result = await _repo.AddAsync(booth, ct);

                // ── Step 4: Both inserts succeeded → commit ───────────────────────
                await _uow.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw new ApplicationException(
                    $"Booth registration failed. Rolled back. Reason: {ex.Message}", ex);
            }
        }
    }
}