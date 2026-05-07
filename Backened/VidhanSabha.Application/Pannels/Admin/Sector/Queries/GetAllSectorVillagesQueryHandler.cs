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
    internal class GetAllSectorVillagesQueryHandler : IRequestHandler<GetAllSectorVillagesQuery, List<VillageDto>>
    {
        private ISectorRepository _sector;

        public GetAllSectorVillagesQueryHandler(ISectorRepository sector)
        {
            _sector = sector;
        }
        public async Task<List<VillageDto>> Handle(GetAllSectorVillagesQuery request, CancellationToken cancellationToken)
        {
            return  await _sector.GetAllSectorVillagesByUserId(request.QueryParams,cancellationToken);
        }
    }
}
