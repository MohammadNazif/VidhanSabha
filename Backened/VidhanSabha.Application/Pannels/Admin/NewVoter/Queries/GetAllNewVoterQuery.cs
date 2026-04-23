using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Queries
{
    public record GetAllNewVoterQuery(NewVoterQueryParams QueryParams):IRequest<PagedResult<NewVoterResponseDto>>
    {

    }
}
