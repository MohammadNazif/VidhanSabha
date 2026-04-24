using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{
    public record GetAllBoothBySectorIdQuery(BoothQueryParams QueryParams):IRequest<PagedResult<BoothResponseDto>>
    {
       
    }
}
