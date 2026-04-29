using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Queries
{
    public record GetAllMemQuery(BoothSamitiQueryParams QueryParams) : IRequest<PagedResult<BoothSamitiMemResponseDto>>
    {
    }
}
