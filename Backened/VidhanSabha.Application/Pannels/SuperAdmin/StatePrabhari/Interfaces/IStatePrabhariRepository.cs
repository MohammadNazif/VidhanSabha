using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Interfaces
{
    public interface IStatePrabhariRepository
    {
        Task<Tbl_StatePrabhari?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<StatePrabhariResponseDto?> GetProfileByUserIdAsync(string  userId,string role, CancellationToken ct = default);
        Task<IReadOnlyList<StatePrabhariResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<bool> EmailExistsAsync(string email, int? excludeId, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_StatePrabhari prabhari, CancellationToken ct = default);
        Task<int> UpdateAsync(Tbl_StatePrabhari prabhari);

        Task<PagedResult<StatePrabhariResponseDto>> GetByStateIdAsync(
         int stateId,
         string userId,
         QueryParams qp,
         CancellationToken ct = default);
        Task SaveAsync(CancellationToken ct = default);
    }
}
