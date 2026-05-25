using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Interface;
using VidhanSabha.Domain.Entities.StatePrabhari;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.StatePrabhari
{
    internal class VidhanSabhaRepository : BaseRepository<Tbl_VidhanSabha>, IVidhanSabhaRepository
    {
        public VidhanSabhaRepository(DatabaseContext context) : base(context)
        {
        }

            public async Task<int> AddAsync(Tbl_VidhanSabha vidhanSabha)
                {
                try
                {
                    await _context.Tbl_VidhanSabha.AddAsync(vidhanSabha);
                    await _context.SaveChangesAsync();
                    return vidhanSabha.Id;
                }
                catch (Exception)
                {
                    throw;
                }
            }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Tbl_VidhanSabha vidhanSabha)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Tbl_VidhanSabha>> IVidhanSabhaRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<VidhanSabhaSatewiseResponseDto>> GetByIdAsync(
      vidhansabhaparams qp,
      int? districtId,
      CancellationToken ct = default)
        {
            var query = _context.Tbl_VidhanSabha
                .AsNoTracking()
                .Where(x => x.UserId == qp.UserId && x.Status &&
                           (districtId == null || x.DistrictId == districtId));

            // Optional search
            Expression<Func<Tbl_VidhanSabha, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();

                int number;
                var isNumber = int.TryParse(term, out number);

                search = x =>
                    x.VidhanSabhaName.ToLower().Contains(term) ||
                    x.district.DistrictName.ToLower().Contains(term) ||
                    (isNumber && x.VidhanSabhaNumber == number);
            }

            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: x => x.district.DistrictName,
                projection: b => new VidhanSabhaSatewiseResponseDto
                {
                    Id = b.Id,
                    DistrictId = b.DistrictId,
                    DistrictName = b.district.DistrictName,
                    VidhanSabhaName = b.VidhanSabhaName,
                    VidhanSabhaNumber = b.VidhanSabhaNumber,
                    HasPrabhari = _context.Tbl_StatePrabhari
                        .Any(p => p.VidhansabhaId == b.Id)
                },
                ct: ct
            );
        }

        public async Task<VidhanSabhaSatewiseResponseDto?> GetByVidhanIdAsync(int vidhanId)
        {
            return await _context.Tbl_VidhanSabha
                .Where(b => b.Id == vidhanId)
                .Select(b => new VidhanSabhaSatewiseResponseDto
                {
                    vidhanSabhaId = b.Id

                })
                .FirstOrDefaultAsync();
        }

        public Task<int> UpdateVidhanSabhaNameNumberAsync(Tbl_VidhanSabha data)
        {
            _context.Tbl_VidhanSabha.Update(data);
            return Task.FromResult(_context.SaveChanges());
        }

        public async  Task<Tbl_VidhanSabha> GetVidhanSabhaByIdAsync(int id)
        {
             return await _context.Tbl_VidhanSabha.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
 