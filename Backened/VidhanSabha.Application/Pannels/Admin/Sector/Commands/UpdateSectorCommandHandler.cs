using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Common.CredentialMananger.Interface;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Commands
{
    public class UpdateSectorCommandHandler : IRequestHandler<UpdateSectorCommand, int>
    {
        private readonly ISectorRepository _sectorRepository;
        private readonly CredentialManagerFunc _credentialManager;
        private readonly IImageService _imageService; 

        public UpdateSectorCommandHandler(ISectorRepository sectorRepository, CredentialManagerFunc credentialManager,IImageService imageService)
        {
            _sectorRepository = sectorRepository;
            _credentialManager = credentialManager;
            _imageService = imageService;
        }

        public async Task<int> Handle(UpdateSectorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var sector = await _sectorRepository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Sector with Id {dto.Id} not found.");

            // ✅ Resolve new image path (null if no image uploaded)
            string? newImagePath = null;
            if (dto.ProfileImage != null)
            {
                //if (!_imageService.IsValidImage(request.ProfileImage))
                //    throw new ValidationException("Invalid image. Only JPG/PNG/WEBP under 5MB allowed.");

                newImagePath = await _imageService.SaveImageAsync(
                    dto.ProfileImage,
                    subFolder: "profiles/sector"
                );

                // Delete old image only after new one saved successfully
                await _imageService.DeleteImageAsync(sector.ProfileImage);
            }

            if (!dto.IsSectorSanyojak)
            {
                // ✅ UpdateBasic clears ProfileImage to null — is that intended?
                // If not, add imagePath param to UpdateBasic too (see below)
                sector.UpdateBasic(dto.MandalId, dto.VillageIds, dto.SectorName);
            }
            else
            {
                if (dto.InchargeName is null || dto.Age is null || dto.FatherName is null ||
                    dto.CategoryId is null || dto.CastId is null ||
                    dto.EducationLevel is null || dto.PhoneNumber is null || dto.Address is null)
                    throw new ArgumentException("All Sanyojak fields are required when IsSectorSanyojak is true.");

                var oldPhone = sector.PhoneNumber;

                
                sector.UpdateWithSanyojak(
                    dto.MandalId,
                    dto.VillageIds,
                    dto.SectorName,
                    dto.InchargeName,
                    dto.Age.Value,
                    dto.FatherName,
                    dto.CategoryId.Value,
                    dto.CastId.Value,
                    dto.EducationLevel,
                    dto.PhoneNumber,
                    dto.Address,
                    newImagePath      
                );

                if (oldPhone != dto.PhoneNumber)
                {
                    await _credentialManager.UpdateCredentialAsync(
                        sector.UserId,
                        dto.PhoneNumber
                    );
                }
            }

            return await _sectorRepository.UpdateAsync(sector);
           
        }
        //private static SectorResponseDto MapToDto(Domain.Entities.Admin.Tbl_Sector s) => new()
        //{
        //    Id = s.Id,
        //    MandalId = s.MandalId,
        //    Villages = s.Villages.Select(v => new VillageResponseDto
        //    {
        //        VillageId = v.VillageId,
        //        VillageName = v.Village.VillageName
        //    }).ToList(),
        //    SectorName = s.SectorName,
        //    IsSectorSanyojak = s.IsSectorSanyojak,
        //    InchargeName = s.InchargeName,
        //    Age = s.Age,
        //    FatherName = s.FatherName,
        //    CategoryId = s.CategoryId,
        //    CastId = s.CastId,
        //    EducationLevel = s.EducationLevel,
        //    PhoneNumber = s.PhoneNumber,
        //    Address = s.Address,
        //    ProfileImage = s.ProfileImage,
        //    Status = s.Status,
        //    CreatedAt = s.CreatedAt,
        //    UpdatedAt = s.UpdatedAt
        //};
    }
}
