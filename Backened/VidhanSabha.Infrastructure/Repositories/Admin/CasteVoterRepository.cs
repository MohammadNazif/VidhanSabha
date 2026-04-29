using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class CasteVoterRepository : BaseRepository<Tbl_CasteVoter>, ICasteVoterRepository
    {
        public CasteVoterRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<int> AddRangeAsync(List<Tbl_CasteVoter> castevoters, CancellationToken ct = default)
        {
            await _context.Tbl_CasteVoter.AddRangeAsync(castevoters);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteRangeAsync(List<Tbl_CasteVoter> castevoters, CancellationToken ct = default)
        {
            _context.Tbl_CasteVoter.RemoveRange(castevoters);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<PagedResult<CasteVoterResponseDto>> GetAllAsync(CasteVoterQueryParams qp, CancellationToken ct = default)
        {
            //var query = _context.Tbl_CasteVoter
            //    .Where(x => x.Status)
            //    .Select(x => new CasteVoterResponseDto
            //    {
            //        Id = x.Id,
            //        CasteVoterId = x.CasteVoterId,

            //        BoothNumber = x.BoothVoter.Booth.BoothNumber,
            //        PollingStationName = x.BoothVoter.Booth.PollingStationName,

            //        SubCasteId = x.SubCasteId,
            //        SubCasteName = x.Cast != null ? x.Cast.CastName : "",
            //        Number = x.Number
            //    });

            //var data = await query.ToListAsync(ct);

            //return new PagedResult<CasteVoterResponseDto>
            //{
            //    Items = data,
            //    TotalCount = data.Count
            //};

            var query = _context.Tbl_CasteVoter
    .AsNoTracking()
    .Where(b =>
    (!qp.Id.HasValue || b.Id == qp.Id) &&
    (!qp.BoothId.HasValue || b.CasteVoterId == qp.BoothId) &&
    (!qp.SectorId.HasValue ||
        _context.Tbl_Booth
            .Where(x => x.Id == b.BoothVoter.BoothId)
            .Select(x => x.SectorId)
            .FirstOrDefault() == qp.SectorId
    )
);

            Expression<Func<Tbl_CasteVoter, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.SubCasteId.Equals(Convert.ToInt32(term)) ||
                    b.CasteVoterId.Equals(Convert.ToInt32(term));
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.BoothVoter.Booth.BoothNumber,
               projection: m => new CasteVoterResponseDto
               {
                   Id = m.Id,
                   CasteVoterId = m.CasteVoterId,
                   BoothNumber = m.BoothVoter.Booth.BoothNumber,
                   PollingStationName = m.BoothVoter.Booth.PollingStationName,
                   SubCasteId = m.SubCasteId,
                   SubCasteName = m.Cast != null ? m.Cast.CastName : "",
                   Number = m.Number
               },
               ct: ct
               );
        }

        public async Task<List<Tbl_CasteVoter>> GetByCasteVoterIdAsync(int boothId, CancellationToken ct = default)
        {
            return await _context.Tbl_CasteVoter
                .Where(x => x.CasteVoterId == boothId && x.Status)
                .ToListAsync(ct);
        }

        public async Task<Tbl_CasteVoter?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Tbl_CasteVoter
                .FirstOrDefaultAsync(x => x.Id == id && x.Status, ct);
        }

        public async Task<int> UpdateRangeAsync(List<Tbl_CasteVoter> castevoters, CancellationToken ct = default)
        {
            _context.Tbl_CasteVoter.UpdateRange(castevoters);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<int> GetTotalVoterByCasteVoterIdAsync(int boothId, CancellationToken ct)
        {
            return await _context.Tbl_BoothVoter
                .Where(x => x.Id == boothId && x.Status)
                .Select(x => x.TotalVoter)
                .FirstOrDefaultAsync(ct);
        }
    }
}