using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;
using VidhanSabha.Application.Pannels.Admin.BDC.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
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

        public async Task<List<BDCResponseDto>> GetAllAsync(int? Id = null, CancellationToken ct = default)
        {
            var result = await _context.Tbl_BDC
                .Select(m => new BDCResponseDto
                {
                    Id = m.Id,
                    Block = m.Block,
                    Name = m.Name,
                    WardNumber=m.WardNumber,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.Name,
                    CastId = m.CastId,
                    CastName = m.Cast.CastName,
                    Age=m.Age,
                    Mobile = m.Mobile,
                    PartyId = m.PartyId,
                    PartyName = m.Party.Party,
                    Education=m.Education,
                    Villages = m.Villages.Select(v => new VillageResponseDto
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName
                    }).ToList(),
                }).ToListAsync();
            return result;
        }
    }
}
