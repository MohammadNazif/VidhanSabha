using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.DoubleVoter.Queries
{
    public record GetAllDoubleVoterQuery(DoubleVoterQueryParams QueryParams) : 
        IRequest<PagedResult<DoubleVoterResponseDto>>
    {
    }
}
