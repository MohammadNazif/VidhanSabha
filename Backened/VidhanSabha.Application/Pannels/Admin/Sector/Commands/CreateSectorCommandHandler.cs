using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Commands
{
    public class CreateSectorCommandHandler : IRequestHandler<CreateSectorCommand, int>
    {
        private readonly IImageService _imageService;
        private readonly ISectorRepository _sectorRepository;
        private readonly CredentialManagerFunc _credentialManager;

        public CreateSectorCommandHandler(
            ISectorRepository sectorRepository,
            CredentialManagerFunc credentialManager,
            IImageService imageService)
        {
            _imageService = imageService;
            _sectorRepository = sectorRepository;
            _credentialManager = credentialManager;

        }

        public async Task<int> Handle(CreateSectorCommand request, CancellationToken cancellationToken)
        {
            var imagePath = await request.Dto.ResolveImageAsync(
            _imageService,
            subFolder: "profiles/sector",
            imageSelector: dto => dto.ProfileImage
        );
            var dto = request.Dto;

            Tbl_Sector sector;

            if (!dto.IsSectorSanyojak)
            {
                // ✅ No Sanyojak
                sector = Tbl_Sector.CreateBasic(
                    request.CreatedById,
                    request.CreatedBy,
                    request.CreatedByUserId,
                    dto.MandalId,
                    dto.VillageIds,
                    dto.SectorName
                );
            }
            else
            {
                // ✅ Validation
                if (dto.InchargeName is null || dto.Age is null || dto.FatherName is null ||
                    dto.CategoryId is null || dto.CastId is null ||
                    dto.PhoneNumber is null)
                    throw new ArgumentException("All Sanyojak fields are required");

                // ✅ Step 1 — Generate UserId
                var userId = $"USR_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

                // ✅ Step 2 — Insert credential
                await _credentialManager.InsertCredentialAsync(
                    userId: userId,
                    mobile: dto.PhoneNumber,
                    email: "",
                    role: Domain.Enums.PrabhariRole.SectorSanyojak
                );

                // ✅ Step 3 — Create sector with Sanyojak
                sector = Tbl_Sector.CreateWithSanyojak(
                    request.CreatedById,
                    request.CreatedBy,
                    request.CreatedByUserId,
                    dto.MandalId,
                    dto.VillageIds,
                    dto.SectorName,
                    userId, 
                    dto.InchargeName,
                    dto.Age.Value,
                    dto.FatherName,
                    dto.CategoryId.Value,
                    dto.CastId.Value,
                    dto.EducationLevel,
                    dto.PhoneNumber,
                    dto.Address,
                    imagePath
                );
            }

            return await _sectorRepository.AddAsync(sector);

            
        }
    }
}