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
    internal class getSectorDashboardCountQueryHandler : IRequestHandler<getSectorDashboardCountQuery, SectorDashboardCountsDto>
    {
        private readonly IDashboard _dashboard;
        public getSectorDashboardCountQueryHandler(IDashboard dashboard)
    => _dashboard = dashboard;
        public Task<SectorDashboardCountsDto> Handle(getSectorDashboardCountQuery request, CancellationToken cancellationToken)
        {
             return _dashboard.GetSectorDashboardCountsAsync(request.UserId);
        }
    }
}
