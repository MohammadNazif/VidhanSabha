using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    public class GetAllSectorsHandler : IRequestHandler<GetAllSectorsQuery, PagedResult<SectorResponseDto>>
    {
        private readonly ISectorRepository _repo;

        public GetAllSectorsHandler(ISectorRepository repo) => _repo = repo;

        public async Task<PagedResult<SectorResponseDto>> Handle(GetAllSectorsQuery request, CancellationToken cancellationToken)
        {
            var sectors = await _repo.GetAllAsync(request.QueryParams,cancellationToken);
            if(sectors==null)
            {
                throw new NotFoundException("Sector Not Found");
            }

            return sectors;
            //.Select(s => new SectorResponseDto
            //{
            //    Id = s.Id,
            //    MandalId = s.MandalId,
            //    MandalName = s.Mandal.Name,
            //    VillageId = s.VillageId,
            //    VillageName = s.Village.VillageName,
            //    SectorName = s.SectorName,
            //    IsSectorSanyojak = s.IsSectorSanyojak,
            //    InchargeName = s.InchargeName,
            //    Age = s.Age,
            //    FatherName = s.FatherName,
            //    CategoryId = s.CategoryId,
            //    CategoryName = s.Category?.Name,
            //    CastId = s.CastId,
            //    CastName = s.Cast?.CastName,
            //    EducationLevel = s.EducationLevel,
            //    PhoneNumber = s.PhoneNumber,
            //    Address = s.Address,
            //    ProfileImage = s.ProfileImage,
            //    Status = s.Status
            //}).ToList();
        }
    }
}
