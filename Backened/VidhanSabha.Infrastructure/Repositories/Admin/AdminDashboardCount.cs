using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Pannels.Admin.Dashboard.Dtos;
using VidhanSabha.Application.Pannels.Admin.Dashboard.Interface;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    internal class AdminDashboardCount : BaseRepository<Tbl_Mandal>, IDashboard
    {
        public AdminDashboardCount(DatabaseContext context) : base(context)
        {
        }

        public async Task<DashboardCountsDto> GetDashboardCountsAsync(
        string userId)
        {
            var vidhanSabhaId = await _context.Tbl_StatePrabhari
                .Where(u => u.userId == userId)
                .Select(u => u.VidhansabhaId)
                .FirstOrDefaultAsync();

            var mandalIds = await _context.Tbl_Mandal
                .Where(m => m.VidhanId == vidhanSabhaId && m.Status)
                .Select(m => m.Id)
                .ToListAsync();

            var boothIds = await _context.Tbl_Booth
                .Where(b => mandalIds.Contains(b.MandalId) && b.Status)
                .Select(b => b.Id)
                .ToListAsync();

            return new DashboardCountsDto
            {
                Mandal = mandalIds.Count,
                Sector = await _context.Tbl_Sector
                                   .CountAsync(x => mandalIds.Contains(x.MandalId) && x.Status),
                Booth = boothIds.Count,
                PannaPramukh = await _context.Tbl_PannaPramukh
                                   .CountAsync(x => boothIds.Contains(x.BoothId) && x.Status),

                Sahmat = await _context.Tbl_SahmatAsahmat.CountAsync(x => boothIds.Contains(x.BoothId) && x.TypeId == 1 && x.Status),
                 Asahmat = await _context.Tbl_SahmatAsahmat.CountAsync(x => boothIds.Contains(x.BoothId) && x.TypeId == 2 && x.Status),
                 Pravasi = await _context.Tbl_PravasiVoter.CountAsync(x => boothIds.Contains(x.BoothId) && x.Status),
                 NewVoters = await _context.Tbl_NewVoter.CountAsync(x => boothIds.Contains(x.BoothId) && x.Status),
                 DoubleVoter = await _context.Tbl_DoubleVoter.CountAsync(x => boothIds.Contains(x.BoothId) && x.Status),
                 PrabhavshaliVyakti = await _context.Tbl_PrabhavshaliVyakti.CountAsync(x => boothIds.Contains(x.BoothId) && x.Status),
                 Block = await _context.Tbl_Block.CountAsync( x => x.Status),
                 BDC  = await _context.Tbl_Block.CountAsync(x => x.Status)



            };
        }
    }
}

