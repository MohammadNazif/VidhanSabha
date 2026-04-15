using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.DTOs;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Application.Pannels.SuperAdmin.designation.Interfaces
{
    public interface IDesignationRepository
    {
        Task<Tbl_Designation> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<DesignationResponseDto>> GetAllAsync(CancellationToken ct = default);
        Task<IReadOnlyList<DesignationResponseDto>> GetByDesignationTypeIdAsync(int designationTypeId, CancellationToken ct = default);
        Task<int> CreateAsync(Tbl_Designation dto, CancellationToken ct = default);
        Task<int> UpdateAsync(Tbl_Designation dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task RestoreAsync(int id, CancellationToken ct = default);
    }
}
