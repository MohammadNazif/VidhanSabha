using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Queries
{
    public record GetAllBDCQuery(BDCQueryParams QueryParams)
        :IRequest<PagedResult<BDCResponseDto>>
    {
    }
}
