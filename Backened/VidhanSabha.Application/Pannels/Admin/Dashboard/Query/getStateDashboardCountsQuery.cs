using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Dashboard.Dtos;
using VidhanSabha.Application.Pannels.Admin.Dashboard.Interface;

namespace VidhanSabha.Application.Pannels.Admin.Dashboard.Query
{

    public class getStateDashboardCountsQuery
  : IRequestHandler<GetStateDashboardCounts, StateDashboardCountsDto>
    {
        private readonly IDashboard _dashboard;
        public getStateDashboardCountsQuery(IDashboard dashboard)
    => _dashboard = dashboard;
        public async Task<StateDashboardCountsDto> Handle(GetStateDashboardCounts request, CancellationToken cancellationToken)
        {
           return  await _dashboard.GetStateDashboardCountsAsync(request.userId);
        }
    }
}
