using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Queries
{
    public record GetAllPannaQuery(PannaPramukhQueryParams QueryParams) : IRequest<PagedResult<PannaPramukhResponseDto>>
    {
    }
}
