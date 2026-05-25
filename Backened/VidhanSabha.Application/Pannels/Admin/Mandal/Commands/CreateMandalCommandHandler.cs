using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Commands.Create
{
    public class CreateMandalCommandHandler
        : IRequestHandler<CreateMandalCommand, MandalResponseDto>
    {
        private readonly IMandalRepository _repo;
        private readonly CredentialManagerFunc _credentialManager;
        private readonly IUnitOfWork _uow;
        private readonly IImageService _imageService;

        public CreateMandalCommandHandler(
            IMandalRepository repo,
            CredentialManagerFunc credentialManager,
            IUnitOfWork uow,
            IImageService imageService)
        {
            _repo = repo;
            _credentialManager = credentialManager;
            _uow = uow;
            _imageService = imageService;
        }

        public async Task<MandalResponseDto> Handle(
            CreateMandalCommand command, CancellationToken ct)
        {
            int? vidhanId = await _repo.GetVidhansabhaIdByuserIdAsync(command.Dto.UserId);

            if (vidhanId is null or <= 0)
                throw new NotFoundException("VidhanSabha not found for the given user");

            var exists = await _repo.ExistsByNameAsync(vidhanId, command.Dto.Name);
            if (exists)
                throw new ValidationException(new Dictionary<string, string[]>
                {
                    { "Name", new[] { "Mandal name already exists for this VidhanId." } }
                });

            // FIX 1: null-guard was on cmd (never null), must be on cmd.Dto.Sanyojak
            var imagePath = await command.ResolveImageAsync(
                _imageService,
                subFolder: "profiles/mandal",
                imageSelector: cmd => cmd.Dto.Sanyojak?.ProfileImagePath
            );

            var userId = $"USR_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

            await _uow.BeginTransactionAsync();
            try
            {
                Tbl_Mandal.Tbl_MandalSanyojak? sanyojak = null;

                if (command.Dto.IsMandalSanyojak && command.Dto.Sanyojak is not null)
                {
                    await _credentialManager.InsertCredentialAsync(
                        userId: userId,
                        mobile: command.Dto.Sanyojak.PhoneNumber,
                        email: "",
                        role: PrabhariRole.MandalPrabhari  // FIX 2: was MandalPrabhari
                    );

                    sanyojak = Tbl_Mandal.Tbl_MandalSanyojak.Create(
                        userId,
                        command.Dto.Sanyojak.InchargeName,
                        command.Dto.Sanyojak.Age,
                        command.Dto.Sanyojak.FatherName,
                        command.Dto.Sanyojak.PhoneNumber,
                        command.Dto.Sanyojak.CategoryId,
                        command.Dto.Sanyojak.CastId,
                        command.Dto.Sanyojak.EducationLevel,
                        command.Dto.Sanyojak.PhoneNumber,
                        command.Dto.Sanyojak.Address,
                        imagePath
                    );
                }

                var mandal = Tbl_Mandal.Create(
                    vidhanId,
                    command.Dto.Name,
                    command.Dto.IsMandalSanyojak,
                    sanyojak);

                await _repo.AddAsync(mandal);
                var result = mandal.Id;

                await _uow.CommitAsync();

                return new MandalResponseDto
                {
                    Id = result,
                    VidhanId = mandal.VidhanId,
                    Name = mandal.Name,
                    Status = mandal.Status
                };
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw new ApplicationException(
                    $"Mandal registration failed. Rolled back. Reason: {ex.Message}", ex);
            }
        }
    }
}