using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.Entities.SuperAdmin;
using static VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Dtos.VidhanshabhaStateWiseCount;

namespace VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Interfaces
{
    public interface IStateWiseVidhanSabhaCountRepository
    {
        Task<Tbl_VidhansabhaStatewiseCount?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<VidhansabhaResponseDto>> GetAllAsync(string? userId,CancellationToken ct = default);
        Task<IReadOnlyList<Tbl_VidhansabhaStatewiseCount>> GetAllActiveAsync(CancellationToken ct = default);
        Task<bool> ExistsAsync(int stateId, int? excludeId, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_VidhansabhaStatewiseCount state, CancellationToken ct = default);
        void Update(Tbl_VidhansabhaStatewiseCount state);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}

