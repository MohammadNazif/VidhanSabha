using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Queries
{
    public record GetAllSeniorDisabledQuery(SeniorDisabledQueryParams QueryParams) : IRequest<PagedResult<SeniorDisabledResponseDto>>
    {
    }
}
