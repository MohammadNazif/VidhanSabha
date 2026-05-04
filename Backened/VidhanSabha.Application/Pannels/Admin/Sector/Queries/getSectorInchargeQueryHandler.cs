using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    internal class getSectorInchargeQueryHandler : IRequestHandler<getSectorInchargeQuery, IReadOnlyList<SectorIncahrgeDto>>
    {
        private ISectorRepository _sec;

        public getSectorInchargeQueryHandler(ISectorRepository sec)
        {
            _sec = sec;
        }
        public async Task<IReadOnlyList<SectorIncahrgeDto>> Handle(getSectorInchargeQuery request, CancellationToken cancellationToken)
        {
             return   await _sec.GetIncahrgeByIdAsync(request.UserId);
        }
    }
}
