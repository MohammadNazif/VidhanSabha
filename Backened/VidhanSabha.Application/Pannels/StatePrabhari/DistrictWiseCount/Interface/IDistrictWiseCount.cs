using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.Entities.StatePrabhari;
using VidhanSabha.Domain.Entities.SuperAdmin;
using static VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Dtos.DistrictWiseCount;
using static VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Dtos.VidhanshabhaStateWiseCount;

namespace VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Interface
{
    public interface IDistrictWiseCount
    {
        Task<IReadOnlyList<VidhansabhaDistrictResponseDto>?> GetByIdAsync(string userId, CancellationToken ct = default);
        Task<IReadOnlyList<VidhansabhaDistrictRequestDto>> GetAllAsync(string? userId, CancellationToken ct = default);
        Task<Tbl_DistrictWiseCount> GetByDistrictIdAsync(int districId,string userId, CancellationToken ct = default);
        Task<bool> ExistsAsync(int stateId, int? excludeId, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_DistrictWiseCount state, CancellationToken ct = default);
        void Update(Tbl_DistrictWiseCount state);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
