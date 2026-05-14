using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;
using VidhanSabha.Application.Pannels.Admin.BDC.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class BDCRepository : BaseRepository<Tbl_BDC>, IBDCRepository
    {
        public BDCRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<int> AddAsync(Tbl_BDC bdc, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_BDC.AddAsync(bdc);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(Tbl_BDC bdc)
        {
            try
            {
                _context.Tbl_BDC.Update(bdc);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_BDC bdc)
        {
            throw new NotImplementedException();
        }

        public async Task<Tbl_BDC?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_BDC
                  .Include(p=>p.Villages)
                 .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagedResult<BDCResponseDto>> GetAllAsync(BDCQueryParams qp, CancellationToken ct = default)
        {
            var query = _context.Tbl_BDC
                .AsNoTracking()
                .Where(b => b.UserId == qp.UserId &&
                    (!qp.Id.HasValue || b.Id == qp.Id) &&
                    (!qp.PartyId.HasValue || b.PartyId == qp.PartyId));

            Expression<Func<Tbl_BDC, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    //b.Block.bloc ToLower().Contains(term) ||
                    b.Name.ToLower().Contains(term) ||
                    b.Cast.CastName.ToLower().Contains(term) ||
                    b.Party.Party.ToLower().Contains(term) ||
                    b.WardNumber.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: b => b.Block,
                projection: m => new BDCResponseDto
                {
                    Id = m.Id,
                    BlockId = m.Block,
                    Block = m.Blocknav.BlockName,
                    Name = m.Name,
                    WardNumber = m.WardNumber,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.Name,
                    CastId = m.CastId,
                    CastName = m.Cast.CastName,
                    Age = m.Age,
                    Mobile = m.Mobile,
                    PartyId = m.PartyId,
                    PartyName = m.Party.Party,
                    Profile = m.Profile,
                    Education = m.Education,
                    Villages = m.Villages.Select(v => new VillageResponseDto
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName
                    }).ToList(),
                },
                ct : ct
                );
        }
    }
}
