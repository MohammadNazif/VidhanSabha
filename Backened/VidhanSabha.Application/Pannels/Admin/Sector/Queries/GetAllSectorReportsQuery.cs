using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    public record GetAllSectorReportsQuery(SectorQueryParams QueryParams):IRequest<PagedResult<SectorReportDto>>
    {
    }
}
