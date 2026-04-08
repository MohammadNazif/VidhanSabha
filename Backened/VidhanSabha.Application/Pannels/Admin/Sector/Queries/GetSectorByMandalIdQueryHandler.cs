using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    
    public class GetSectorByMandalIdQueryHandler : IRequestHandler<GetSectorByMandalIdQuery, List<SectorByMAndalResponseDto>>
    {
        private readonly ISectorRepository _repo;
        public GetSectorByMandalIdQueryHandler(ISectorRepository repo) => _repo = repo;
        public async Task<List<SectorByMAndalResponseDto>> Handle(GetSectorByMandalIdQuery request, CancellationToken cancellationToken)
        {
            var sector = await _repo.GetByMandalIdAsync(request.Id)
                ?? throw new NotFoundException($"Sector with Mandal Id {request.Id} not found.");
           return sector.Select(c => new SectorByMAndalResponseDto
           {
                SectorId = c.Id,
                SectorName = c.SectorName,
                MandalId = c.MandalId
            }).ToList();
           
        }
    }
}
