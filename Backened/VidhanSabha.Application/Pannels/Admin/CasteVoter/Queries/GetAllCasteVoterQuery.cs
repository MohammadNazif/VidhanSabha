using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.CasteVoter.Queries
{
    public record GetAllCasteVoterQuery: IRequest<PagedResult<CasteVoterResponseDto>>
    {
    }
}
