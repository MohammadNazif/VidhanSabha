using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class SectorRepository : BaseRepository<Tbl_Sector>, ISectorRepository
    {
        public SectorRepository(DatabaseContext context) : base(context) { }

        public async Task<List<Tbl_Sector>> GetAllAsync()
            => await _context.Tbl_Sector
                .Where(s => s.Status)
                .Include(s => s.Mandal)
                .Include(s => s.Village)
                .Include(s => s.Category)
                .Include(s => s.Cast)
                .ToListAsync();

        public async Task<Tbl_Sector?> GetByIdAsync(int id)
            => await _context.Tbl_Sector
                .Include(s => s.Mandal)
                .Include(s => s.Village)
                .Include(s => s.Category)
                .Include(s => s.Cast)
                .FirstOrDefaultAsync(s => s.Id == id && s.Status);
        public async Task<List<Tbl_Sector>?> GetByMandalIdAsync(int id)
      => await _context.Tbl_Sector
          .Where(s => s.MandalId == id && s.Status)
          .Include(s => s.Mandal)
          .Include(s => s.Village)
          .Include(s => s.Category)
          .Include(s => s.Cast)
            .ToListAsync();
        
        public async Task AddAsync(Tbl_Sector sector)
        {
            await _context.Tbl_Sector.AddAsync(sector);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tbl_Sector sector)
        {
            _context.Tbl_Sector.Update(sector);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tbl_Sector sector)
        {
            sector.Delete();
            _context.Tbl_Sector.Update(sector);
            await _context.SaveChangesAsync();
        }
    }
}
