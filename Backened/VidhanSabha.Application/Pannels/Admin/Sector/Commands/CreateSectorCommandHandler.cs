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
    public class CreateSectorCommandHandler : IRequestHandler<CreateSectorCommand, SectorResponseDto>
    {
        private readonly ISectorRepository _sectorRepository;

        //private readonly IWebHostEnvironment _env;
        public CreateSectorCommandHandler(ISectorRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
            //_env = env;
        }

        public async Task<SectorResponseDto> Handle(CreateSectorCommand request, CancellationToken cancellationToken)
        {
            //string? imagePath = null;

            //if (request.profile != null)
            //{
            //    var uploads = Path.Combine(_env.WebRootPath, "uploads", "sectors");
            //    Directory.CreateDirectory(uploads);
            //    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ProfileImage.FileName)}";
            //    var filePath = Path.Combine(uploads, fileName);
            //    using var stream = new FileStream(filePath, FileMode.Create);
            //    await request.ProfileImage.CopyToAsync(stream, cancellationToken);
            //    imagePath = $"/uploads/sectors/{fileName}";
            //}
            var dto = request.Dto;

            Tbl_Sector sector;

            if (!dto.IsSectorSanyojak)
            {
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
                if (dto.InchargeName is null || dto.Age is null || dto.FatherName is null ||
                    dto.CategoryId is null || dto.CastId is null ||
                    dto.EducationLevel is null || dto.PhoneNumber is null || dto.Address is null)
                    throw new ArgumentException("All Sanyojak fields are required when IsSectorSanyojak is true.");

                sector = Tbl_Sector.CreateWithSanyojak(
                    request.CreatedById,
                    request.CreatedBy,
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
