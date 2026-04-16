using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.DTOs;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.Interfaces;
using VidhanSabha.Domain.Entities.SuperAdmin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.SuperAdmin
{
    public class DesignationRepository : BaseRepository<Tbl_Designation>, IDesignationRepository
    {
        public DesignationRepository(DatabaseContext context) : base(context)
        {   
        }
        public Task<int> CreateAsync(Tbl_Designation request, CancellationToken ct = default)
        {
             _context.Tbl_Designation.Add(request);
            return _context.SaveChangesAsync(ct);
        }

        public Task DeleteAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<DesignationResponseDto>> GetAllAsync(string userId,CancellationToken ct = default)
        {
            var data = await _context.Tbl_Designation.Where(x=>x.UserId == userId)
                .Select(b => new DesignationResponseDto
                {
                    Id = b.Id,
                    DesignationName = b.DesignationName,
                    //DesignationTypeId = b.DesignationTypeId,
                    //DesignationTypeName = b.DesignationType != null
                     //? b.DesignationType.DesignationName
                     //: null
                })
                .ToListAsync(ct);

            return data;
        }

        public Task<IReadOnlyList<DesignationResponseDto>> GetByDesignationTypeIdAsync(int designationTypeId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Tbl_Designation> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return  await _context.Tbl_Designation.Where(m => m.Id == id).FirstOrDefaultAsync();
            
        }

        public Task RestoreAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Tbl_Designation req, CancellationToken ct = default)
        {
            _context.Tbl_Designation.Update(req);
            return _context.SaveChangesAsync(ct);
        }
    }
}
