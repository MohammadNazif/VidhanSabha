using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Command
{
    public class updateBoothCommandHandler : IRequestHandler<updateBoothCommand, bool>
    {
        private readonly IBoothRepository _repo;
        private readonly CredentialManagerFunc _credentialManager;
        private readonly IUnitOfWork _uow;

        public updateBoothCommandHandler(
            IBoothRepository repo,
            CredentialManagerFunc credentialManager,
            IUnitOfWork uow)
        {
            _repo = repo;
            _credentialManager = credentialManager;
            _uow = uow;
        }

        public async Task<bool> Handle(updateBoothCommand request, CancellationToken ct)
        {
            var dto = request.Dto;

            await _uow.BeginTransactionAsync();

            try
            {
                // ✅ Load Booth Aggregate
                var booth = await _repo.GetByIdAsync(dto.id, ct);

                if (booth == null)
                    throw new Exception($"Booth with Id {dto.id} not found");

                // ✅ Build Villages
                var villages = dto.Villages
                    .Select(v => Tbl_BoothVillage.Create(v.VillageId, v.HasAnshik))
                    .ToList();

                Tbl_BoothSanyojak? sanyojak = null;

                // =========================
                // ✅ SANYOJAK LOGIC
                // =========================
                if (dto.IsBoothSanyojak)
                {
                    if (dto.Sanyojak != null)
                    {
                        // 🆕 CASE: CREATE NEW SANYOJAK
                        if (booth.Sanyojak == null)
                        {
                            var userId = $"USR_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

                            await _credentialManager.InsertCredentialAsync(
                                userId: userId,
                                mobile: dto.Sanyojak.PhoneNumber,
                                email: "",
                                role: PrabhariRole.BoothSanyojak
                            );

                            sanyojak = Tbl_BoothSanyojak.Create(
                                userId,
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
                        else
                        {
                          

                            var oldPhone = booth.Sanyojak.PhoneNumber;

                            // Update DOMAIN
                            booth.Sanyojak.UpdateProfile(
                                dto.Sanyojak.InchargeName,
                                dto.Sanyojak.Age,
                                dto.Sanyojak.FatherName,
                                dto.Sanyojak.CategoryId,
                                dto.Sanyojak.CastId,
                                dto.Sanyojak.EducationLevel,
                                dto.Sanyojak.PhoneNumber,
                                dto.Sanyojak.Address
                            );

                            sanyojak = booth.Sanyojak;

                            // 🔥 Sync LOGIN if mobile changed
                            if (oldPhone != dto.Sanyojak.PhoneNumber)
                            {
                                await _credentialManager.UpdateCredentialAsync(
                                    booth.Sanyojak.UserId,
                                    dto.Sanyojak.PhoneNumber
                                );
                            }
                        }
                    }
                    else
                    {
                        // 🟡 CASE: BOOTH ONLY UPDATE (KEEP EXISTING SANYOJAK)
                        sanyojak = booth.Sanyojak;
                    }
                }
                else
                {
                    // ❌ CASE: REMOVE SANYOJAK
                    sanyojak = null;
                }

                // =========================
                // ✅ UPDATE BOOTH AGGREGATE
                // =========================
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

          
                await _repo.UpdateAsync(booth, ct);

                await _uow.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw new ApplicationException($"Update failed: {ex.Message}", ex);
            }
        }
    }
}