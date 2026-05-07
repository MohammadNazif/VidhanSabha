using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    public class GetAllSectorVillagesQuery : IRequest<List<VillageDto>>
    {
        public SectorVillageQueryParams QueryParams { get; set; }
        public GetAllSectorVillagesQuery(SectorVillageQueryParams q)
        {
            QueryParams = q;
        }
    }
}
