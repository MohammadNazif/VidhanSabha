using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.NewFolder.Interface
{
    public interface IMemberModulePermissionRepository
    {
        Task<Tbl_MemberModulePermission?> GetAsync(int memberId, string module,
                                           CancellationToken ct = default);

        Task<List<Tbl_MemberModulePermission>> GetAllByMemberAsync(int memberId,
                                                                CancellationToken ct = default);

        Task<bool> ExistsAsync(int memberId, string module,
                               CancellationToken ct = default);

        void Add(Tbl_MemberModulePermission permission);
        void Remove(Tbl_MemberModulePermission permission);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
