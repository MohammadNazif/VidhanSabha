using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Queries
{
    public record GetAllCombinedMandalReportsQuery(MandalQueryParams QueryParams):IRequest<PagedResult<CombinedMandalReportDto>>
    {
    }
}
