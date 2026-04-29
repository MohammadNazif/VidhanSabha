using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{
    public record GetAllBoothReportsQuery(BoothQueryParams QueryParams ,string userId) : IRequest<PagedResult<BoothReportsDto>>
    {

    }
}
