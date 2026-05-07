using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Dashboard.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Dashboard.Interface
{
    public interface IDashboard
    {
        Task<DashboardCountsDto> GetDashboardCountsAsync(string userId);

        Task<StateDashboardCountsDto> GetStateDashboardCountsAsync(string userId);

        Task<BoothDashboardCountsDto> GetBoothDashboardCountsAsync(string userId);

        Task<SectorDashboardCountsDto> GetSectorDashboardCountsAsync(string userId);
    }
}
