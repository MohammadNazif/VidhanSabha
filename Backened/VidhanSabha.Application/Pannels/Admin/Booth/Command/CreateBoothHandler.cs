using DocumentFormat.OpenXml.Drawing.Charts;
using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Command
{
    public class CreateBoothHandler : IRequestHandler<CreateBoothCommand, int>
    {
        private readonly IBoothRepository _repo;
        private readonly CredentialManagerFunc _credentialManager;
        private readonly IUnitOfWork _uow;
        private readonly IImageService _imageService;
        private readonly IBoothRepository _booth;

        public CreateBoothHandler(
            IBoothRepository repo,
            CredentialManagerFunc credentialManager,
            IUnitOfWork uow,
            IImageService imageService,IBoothRepository
             booth)
        {
            _repo = repo;
            _credentialManager = credentialManager;
            _uow = uow;
            _imageService = imageService;
            _booth = booth;
        }

        public async Task<int> Handle(CreateBoothCommand request, CancellationToken ct)
        {
            var cmd = request.Dto;
            await _uow.BeginTransactionAsync();
            try
            {

               
                string createdsectorUserId = null;
                if (IsUserRole(request.Role, PrabhariRole.VidhanSabhaPrabhari))
                {
                    createdsectorUserId = await _booth.GetSectorUseridbySectorId(request.Dto.SectorId);
                   
                }
                else if (IsUserRole(request.Role, PrabhariRole.SectorSanyojak))
                {

                    createdsectorUserId = request.UserId;

                    request.UserId = await _booth.GetadminUseridbySectorUserId(request.Dto.SectorId);

                }
                // ── Step 1: Build villages (no DB call, just domain objects) ──────
                var villages = cmd.Villages
                    .Select(v => Tbl_BoothVillage.Create(v.VillageId, v.HasAnshik))
                    .ToList();

                // ── Step 2: Handle Sanyojak credential + entity ───────────────────
                Tbl_BoothSanyojak? sanyojak = null;

                if (cmd.IsBoothSanyojak && cmd.Sanyojak is not null)
                {

                    var imagePath = await request.Dto.ResolveImageAsync(
                    _imageService,
                     subFolder: "profiles/booth",
                    imageSelector: dto => dto.Sanyojak.ProfileImagePath
                    );
                    var userId = $"USR_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
                    // Step 2a — Insert credential (tbl_login) first, same as Prabhari
                    await _credentialManager.InsertCredentialAsync(
                        userId: userId,
                        mobile: cmd.Sanyojak.PhoneNumber,
                        email: "",
                        role:PrabhariRole.BoothSanyojak
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
                        cmd.Sanyojak.Address,
                        imagePath   
                    );
                }

                // ── Step 3: Build Booth aggregate and persist ─────────────────────
                var booth = Tbl_Booth.Create(
                    request.UserId,
                    request.Role,
                    createdsectorUserId,
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
                    $"{ex.Message}", ex);
            }
        }
        private bool IsUserRole(string currentRole, PrabhariRole roleToCheck)
        {
            return currentRole == roleToCheck.ToString();
        }
    }
}