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
    internal class getBoothDashboardCountQueryHandler : IRequestHandler<GetBoothDashboardCountQuery, BoothDashboardCountsDto>
    {
        private readonly IDashboard _dashboard;
        public getBoothDashboardCountQueryHandler(IDashboard dashboard)
    => _dashboard = dashboard;
        public Task<BoothDashboardCountsDto> Handle(GetBoothDashboardCountQuery request, CancellationToken cancellationToken)
        {
            return _dashboard.GetBoothDashboardCountsAsync(request.userId);
        }
    }
}
