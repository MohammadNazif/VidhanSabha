using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Commands
{
    public class UpdateSectorCommandHandler : IRequestHandler<UpdateSectorCommand, SectorResponseDto>
    {
        private readonly ISectorRepository _sectorRepository;
      

        public UpdateSectorCommandHandler(ISectorRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
         
        }

        public async Task<SectorResponseDto> Handle(UpdateSectorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var sector = await _sectorRepository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Sector with Id {dto.Id} not found.");
      
            if (!dto.IsSectorSanyojak)
            {
                sector.UpdateBasic(dto.MandalId, dto.VillageId, dto.SectorName);
            }
            else
            {
                if (dto.InchargeName is null || dto.Age is null || dto.FatherName is null ||
                    dto.CategoryId is null || dto.CastId is null ||
                    dto.EducationLevel is null || dto.PhoneNumber is null || dto.Address is null)
                    throw new ArgumentException("All Sanyojak fields are required when IsSectorSanyojak is true.");

                sector.UpdateWithSanyojak(
                    dto.MandalId,
                    dto.VillageId,
                    dto.SectorName,
                    dto.InchargeName,
                    dto.Age.Value,
                    dto.FatherName,
                    dto.CategoryId.Value,
                    dto.CastId.Value,
                    dto.EducationLevel,
                    dto.PhoneNumber,
                    dto.Address,
                    dto.ProfileImage
                );
            }

            return MapToDto(sector);
        }

        private static SectorResponseDto MapToDto(Domain.Entities.Admin.Tbl_Sector s) => new()
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
