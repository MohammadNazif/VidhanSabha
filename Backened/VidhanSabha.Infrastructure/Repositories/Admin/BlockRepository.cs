using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class BlockRepository:BaseRepository<Tbl_Block>,IBlockRepository
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

        public async Task<List<BlockResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default)
        {
            var result = await _context.Tbl_Block
                .Select(m => new BlockResponseDto
                {
                    Id = m.Id,
                    BlockName = m.BlockName,
                    BlockPramukh=m.BlockPramukh,
                    PartyId=m.PartyId,
                    Party=m.Party.Party,
                    Mobile = m.Mobile,
                    Address = m.Address,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.Name,
                    CastId = m.CastId,
                    CastName = m.Cast.CastName,
                    OccupationId = m.OccupationId,
                    Occupation = m.Occupation.Occupation,
                }).ToListAsync();
            return result;
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
