using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{
   
    public record GetAllBoothsQuery(BoothQueryParams QueryParams,string userId)
        : IRequest<PagedResult<BoothResponseDto>>;
}

