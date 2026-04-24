using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Queries
{
    public record GetAllMandalsQuery(MandalQueryParams QueryParams,string UserId) : IRequest<PagedResult<MandalResponseDto>>
    {
    }
}
