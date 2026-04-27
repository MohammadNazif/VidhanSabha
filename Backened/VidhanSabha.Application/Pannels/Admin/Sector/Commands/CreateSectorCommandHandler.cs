using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Commands
{
    public class CreateSectorCommandHandler : IRequestHandler<CreateSectorCommand, SectorResponseDto>
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

        public async Task<SectorResponseDto> Handle(CreateSectorCommand request, CancellationToken cancellationToken)
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
                    dto.MandalId,
                    dto.VillageId,
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
                    dto.MandalId,
                    dto.VillageId,
                    dto.SectorName,
                    userId, // 👈 IMPORTANT
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

            await _sectorRepository.AddAsync(sector);

            return MapToDto(sector);
        }

        private static SectorResponseDto MapToDto(Tbl_Sector s) => new()
        {
            Id = s.Id,
            MandalId = s.MandalId,
            VillageId = s.VillageId,
            SectorName = s.SectorName,
            IsSectorSanyojak = s.IsSectorSanyojak,
            InchargeName = s.InchargeName,
            Age = s.Age,
            FatherName = s.FatherName,
            CategoryId = s.CategoryId,
            CastId = s.CastId,
            EducationLevel = s.EducationLevel,
            PhoneNumber = s.PhoneNumber,
            Address = s.Address,
            ProfileImage = s.ProfileImage,
            Status = s.Status,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };
    }
}