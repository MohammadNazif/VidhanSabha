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
    public class GetDashboardCountsHandler
    : IRequestHandler<GetDashboardCountsQuery, DashboardCountsDto>
    {
        private readonly IDashboard _dashboard;
        public GetDashboardCountsHandler(IDashboard dashboard)
    => _dashboard = dashboard;
        public async Task<DashboardCountsDto> Handle(GetDashboardCountsQuery request, CancellationToken cancellationToken)
        {
            var counts = await _dashboard.GetDashboardCountsAsync(request.userId);
            return counts;
        }
    }
}
