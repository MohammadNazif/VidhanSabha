using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.MemberModulePermission.Dtos;
using VidhanSabha.Application.Common.NewFolder.Interface;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    internal class MemberModulePermissionRepository : BaseRepository<Tbl_MemberModulePermissions>, IMemberModulePermissionRepository
    {
        public MemberModulePermissionRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<int> Add(Tbl_MemberModulePermissions permission)
        {
            try
            {
                _context.Tbl_MemberModulePermissions.Add(permission);
                return await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<int> AddRangeAsync(List<Tbl_MemberModulePermissions> entities)
        {
             await _context.Tbl_MemberModulePermissions.AddRangeAsync(entities);
            return await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(int memberId, string module, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tbl_MemberModulePermissions>> GetAllByMemberAsync(int memberId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Tbl_MemberModulePermissions?> GetAsync(int memberId, string module, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task< IReadOnlyList<MemberModulePermissionResDto>> GetPermissionByUserIdAsync(string userId, CancellationToken ct = default)
        {
             return await _context.Tbl_MemberModulePermissions.Where(x => x.MemberId == userId).Select(b => new MemberModulePermissionResDto
            {
                Module = b.Module,
                hasPermission = b.hasPermission
            }).ToListAsync(ct);
        }  

        public void Remove(Tbl_MemberModulePermissions permission)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
        public async Task<int> UpdateRangeAsync(List<Tbl_MemberModulePermissions> entities)
        {
            try
            {
                var memberId = entities.First().MemberId;

                // Fetch existing tracked entities from DB
                var existing = await _context.Tbl_MemberModulePermissions
                    .Where(x => x.MemberId == memberId)
                    .ToListAsync();

                foreach (var existingItem in existing)
                {
                    var updated = entities.FirstOrDefault(e => e.Module == existingItem.Module);
                    if (updated != null)
                    {
                        existingItem.UpdateProperties(updated.hasPermission); // update on tracked entity
                    }
                }

                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
