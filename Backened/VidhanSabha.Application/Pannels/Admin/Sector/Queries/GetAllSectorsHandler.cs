using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    public class GetAllSectorsHandler : IRequestHandler<GetAllSectorsQuery, List<SectorResponseDto>>
    {
        private readonly ISectorRepository _repo;

        public GetAllSectorsHandler(ISectorRepository repo) => _repo = repo;

        public async Task<List<SectorResponseDto>> Handle(GetAllSectorsQuery request, CancellationToken cancellationToken)
        {
            var sectors = await _repo.GetAllAsync();

            return sectors.Select(s => new SectorResponseDto
            {
                Id = s.Id,
                MandalName = s.Mandal.Name,
                VillageName = s.Village.VillageName,
                SectorName = s.SectorName,
                IsSectorSanyojak = s.IsSectorSanyojak,
                InchargeName = s.InchargeName,
                Age = s.Age,
                FatherName = s.FatherName,
                CategoryName = s.Category?.Name,
                CastName = s.Cast?.CastName,
                EducationLevel = s.EducationLevel,
                PhoneNumber = s.PhoneNumber,
                Address = s.Address,
                ProfileImage = s.ProfileImage,
                Status = s.Status
            }).ToList();
        }
    }
}
