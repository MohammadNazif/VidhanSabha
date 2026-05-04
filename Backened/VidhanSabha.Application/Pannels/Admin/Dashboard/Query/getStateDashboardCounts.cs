using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Dashboard.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Dashboard.Query
{
    public record GetStateDashboardCounts(string userId)
        : IRequest<StateDashboardCountsDto>;
}
