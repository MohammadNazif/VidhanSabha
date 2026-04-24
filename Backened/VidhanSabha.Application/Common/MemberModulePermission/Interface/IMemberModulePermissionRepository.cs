using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.MemberModulePermission.Dtos;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.NewFolder.Interface
{
    public interface IMemberModulePermissionRepository
    {
        Task<Tbl_MemberModulePermissions?> GetAsync(int memberId, string module,
                                           CancellationToken ct = default);

        Task<List<Tbl_MemberModulePermissions>> GetAllByMemberAsync(int memberId,
                                                                CancellationToken ct = default);


        Task<IReadOnlyList<MemberModulePermissionResDto>> GetPermissionByUserIdAsync(string userId,
                                                            CancellationToken ct = default);
        Task<bool> ExistsAsync(int memberId, string module,
                               CancellationToken ct = default);

        Task<int> Add(Tbl_MemberModulePermissions permission);
        void Remove(Tbl_MemberModulePermissions permission);
        Task<int> AddRangeAsync(List<Tbl_MemberModulePermissions> entities);

        Task<int> UpdateRangeAsync(List<Tbl_MemberModulePermissions> entities);
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
