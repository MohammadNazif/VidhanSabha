using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Commands
{
    public class UpdateMandalCommandHandler : IRequestHandler<UpdateMandalCommand, MandalResponseDto>
    {
        private readonly IMandalRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IImageService _imageService;
        private readonly CredentialManagerFunc _credentialManager;

        public UpdateMandalCommandHandler(
            IMandalRepository repo,
            IUnitOfWork uow,
            IImageService imageService,
            CredentialManagerFunc credentialManager)
        {
            _repo = repo;
            _uow = uow;
            _imageService = imageService;
            _credentialManager = credentialManager;
        }

        public async Task<MandalResponseDto> Handle(UpdateMandalCommand req, CancellationToken cancellationToken)
        {
            await _uow.BeginTransactionAsync();

            try
            {
                var request = req.Dto;

                var mandal = await _repo.GetByIdAsync(request.Id);
                if (mandal == null)
                    throw new NotFoundException("Mandal not found");

                Tbl_Mandal.Tbl_MandalSanyojak? sanyojak = null;

                // =========================
                // 🔹 SANYOJAK LOGIC
                // =========================
                if (request.IsMandalSanyojak)
                {
                    if (request.Sanyojak != null)
                    {
                        if (mandal.Sanyojak == null)
                        {
                            // CREATE NEW SANYOJAK
                            var imagePath = await request.Sanyojak.ResolveImageAsync(
                                _imageService,
                                subFolder: "profiles/mandal",
                                imageSelector: dto => dto.ProfileImagePath
                            );

                            var userId = $"USR_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

                            await _credentialManager.InsertCredentialAsync(
                                userId: userId,
                                mobile: request.Sanyojak.PhoneNumber,
                                email: "",
                                role: Domain.Enums.PrabhariRole.MandalPrabhari
                            );

                            sanyojak = Tbl_Mandal.Tbl_MandalSanyojak.Create(
                                userId,
                                request.Sanyojak.InchargeName,
                                request.Sanyojak.Age,
                                request.Sanyojak.FatherName,
                                request.Sanyojak.PhoneNumber,
                                request.Sanyojak.CategoryId,
                                request.Sanyojak.CastId,
                                request.Sanyojak.EducationLevel,
                                request.Sanyojak.PhoneNumber,
                                request.Sanyojak.Address,
                                imagePath
                            );
                        }
                        else
                        {
                            // UPDATE EXISTING SANYOJAK
                            var oldPhone = mandal.Sanyojak.PhoneNumber;

                            // FIX 3: capture existing path BEFORE any mutation
                            var existingImagePath = mandal.Sanyojak.ProfileImagePath;
                            string? newImagePath = null;

                            if (request.Sanyojak.ProfileImagePath != null)
                            {
                                newImagePath = await _imageService.SaveImageAsync(
                                    request.Sanyojak.ProfileImagePath,
                                    subFolder: "profiles/mandal"
                                );

                                await _imageService.DeleteImageAsync(existingImagePath);
                            }

                            mandal.Sanyojak.UpdateProfile(
                                request.Sanyojak.InchargeName,
                                request.Sanyojak.Age,
                                request.Sanyojak.FatherName,
                                request.Sanyojak.CategoryId,
                                request.Sanyojak.CastId,
                                request.Sanyojak.EducationLevel,
                                request.Sanyojak.PhoneNumber,
                                request.Sanyojak.Address,
                                newImagePath ?? existingImagePath  // FIX 2: preserve existing path when no new image
                            );

                            sanyojak = mandal.Sanyojak;

                            if (oldPhone != request.Sanyojak.PhoneNumber)
                            {
                                await _credentialManager.UpdateCredentialAsync(
                                    mandal.Sanyojak.UserId,
                                    request.Sanyojak.PhoneNumber
                                );
                            }
                        }
                    }
                    else
                    {
                        // Keep existing Sanyojak
                        sanyojak = mandal.Sanyojak;
                    }
                }
                // else: sanyojak remains null → removes the relationship

                // =========================
                // 🔹 UPDATE MANDAL
                // =========================
                // FIX 1: use the domain method — avoids illegal private-set assignments
                mandal.Update(request.Name, request.IsMandalSanyojak, sanyojak);

                await _repo.UpdateAsync(mandal);
                await _uow.CommitAsync();

                return new MandalResponseDto
                {
                    Id = mandal.Id,
                    VidhanId = mandal.VidhanId,
                    Name = mandal.Name,
                    Status = mandal.Status
                };
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw new ApplicationException($"Mandal update failed: {ex.Message}", ex);
            }
        }
    }
}