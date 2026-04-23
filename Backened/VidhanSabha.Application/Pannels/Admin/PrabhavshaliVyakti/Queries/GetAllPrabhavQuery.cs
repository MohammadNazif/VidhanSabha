using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Queries
{
    public record GetAllPrabhavQuery(PrabhavshaliQueryParams QueryParams) : IRequest<PagedResult<PrabhavshaliResponseDto>>
    {
    }
}
