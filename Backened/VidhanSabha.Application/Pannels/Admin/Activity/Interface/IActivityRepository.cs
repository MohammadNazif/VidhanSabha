using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Activity.Dtos;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Activity.Interface
{
    public interface IActivityRepository
    {
        Task<int> AddAsync(Tbl_Activity entity, CancellationToken ct = default);
        Task<Tbl_Activity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagedResult<ActivityResponseDto>> GetAllActiveAsync(ActivityQueryParams qp,CancellationToken ct = default);
        Task UpdateAsync(Tbl_Activity entity, CancellationToken ct = default);
        Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    }
}
