using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.NewFolder.Interface;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    internal class MemberModulePermissionRepository : BaseRepository<Tbl_MemberModulePermission>, IMemberModulePermissionRepository
    {
        public MemberModulePermissionRepository(DatabaseContext context) : base(context)    
        {
            
        }
        public void Add(Tbl_MemberModulePermission permission)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int memberId, string module, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tbl_MemberModulePermission>> GetAllByMemberAsync(int memberId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Tbl_MemberModulePermission?> GetAsync(int memberId, string module, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(Tbl_MemberModulePermission permission)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
