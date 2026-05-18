using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;
using static VidhanSabha.Application.Common.ExportPdfExcel.Dtos.BlockExportDef;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class BlockRepository : BaseRepository<Tbl_Block>, IBlockRepository
    {
        public BlockRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<int> AddAsync(Tbl_Block block, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_Block.AddAsync(block);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(Tbl_Block block)
        {
            try
            {
                _context.Tbl_Block.Update(block);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_Block block)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BlockNameResponse>> GetAllBlockNameAsync(string? userId = null, CancellationToken ct = default)
        {
            var result = await _context.Tbl_Block.Where(x => x.UserId == userId)
                .Select(m => new BlockNameResponse
                {
                    Id = m.Id,
                    BlockName = m.BlockName
                }).ToListAsync();
            return result;
        }

        public async Task<PagedResult<BlockResponseDto>> GetAllAsync(BlockQueryParams qp, CancellationToken ct = default)
        {
            var query = _context.Tbl_Block
                .AsNoTracking()
                .Where(b =>
                b.UserId == qp.UserId &&
                    (!qp.Id.HasValue || b.Id == qp.Id) &&
                    (!qp.CastId.HasValue || b.Cast.Id == qp.CastId));

            Expression<Func<Tbl_Block, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.BlockName.ToLower().Contains(term) ||
                    b.BlockPramukh.ToLower().Contains(term) ||
                    b.Mobile.ToLower().Contains(term) ||
                    b.Cast.CastName.ToLower().Contains(term) ||
                    b.Occupation.Occupation.ToLower().Contains(term) ||
                    b.Party.Party.ToLower().Contains(term) ||
                    b.Address.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
                 queryParams: qp,
                 searchPredicate: search,
                 defaultSort: b => b.BlockName,
                 projection: m => new BlockResponseDto
                 {
                     Id = m.Id,
                     BlockName = m.BlockName,
                     BlockPramukh = m.BlockPramukh,
                     PartyId = m.PartyId,
                     Party = m.Party.Party,
                     Mobile = m.Mobile,
                     Address = m.Address,
                     CategoryId = m.CategoryId,
                     CategoryName = m.Category.Name,
                     CastId = m.CastId,
                     CastName = m.Cast.CastName,
                     OccupationId = m.OccupationId,
                     Occupation = m.Occupation.Occupation,
                     Profile = m.Profile
                 },
                 ct: ct
                 );
        }

        public async Task<List<BlockExportRow>> GetBlockExportAsync(BlockExportFilter qp)
        {
            var query = _context.Tbl_Block
                .AsNoTracking()
                .Where(b =>
                    b.UserId == qp.UserId &&
                    (!qp.Id.HasValue || b.Id == qp.Id) &&
                    (!qp.CastId.HasValue || b.Cast.Id == qp.CastId)
                );

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                query = query.Where(b =>
                    b.BlockName.ToLower().Contains(term) ||
                    b.BlockPramukh.ToLower().Contains(term) ||
                    b.Mobile.ToLower().Contains(term) ||
                    b.Cast.CastName.ToLower().Contains(term) ||
                    b.Occupation.Occupation.ToLower().Contains(term) ||
                    b.Party.Party.ToLower().Contains(term) ||
                    b.Address.ToLower().Contains(term)
                );
            }

            return await query.Select(m => new BlockExportRow
            {
                BlockName = m.BlockName,
                BlockPramukh = m.BlockPramukh,
                Party = m.Party.Party,
                Mobile = m.Mobile,
                Address = m.Address,
                Category = m.Category.Name,
                Cast = m.Cast.CastName,
                Occupation = m.Occupation.Occupation,
            }).ToListAsync();
        }



        public async Task<Tbl_Block?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_Block
                 .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
