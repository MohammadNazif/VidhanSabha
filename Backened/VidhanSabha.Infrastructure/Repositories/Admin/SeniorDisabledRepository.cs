using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Common.SeniorDisabledType.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class SeniorDisabledRepository: BaseRepository<Tbl_SeniorDisabled>, ISeniorDisabledRepository
    {
        public SeniorDisabledRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<int> AddAsync(List<Tbl_SeniorDisabled> seniordisabled, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_SeniorDisabled.AddRangeAsync(seniordisabled);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(Tbl_SeniorDisabled seniordisabled)
        {
            try
            {
                _context.Tbl_SeniorDisabled.Update(seniordisabled);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_SeniorDisabled seniordisabled)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<SeniorDisabledResponseDto>> GetAllAsync(SeniorDisabledQueryParams qp, CancellationToken ct = default)
        {
            var query = _context.Tbl_SeniorDisabled
               .AsNoTracking()
               .AsQueryable();

            var boothIds = qp.GetBoothIds();
            var CastIds = qp.GetCastIds();
            var villageIds = qp.GetVillageIds();

            // ✅ FIX 1: query = assign karo, sirf query.Where nahi
            query = query.Where(b =>
             
                (!qp.Id.HasValue || b.Id == qp.Id) &&
                 (!qp.TypeId.HasValue || b.TypeId == qp.TypeId) &&
                
                (b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId ) && b.Status );            

            if (boothIds.Any())
                query = query.Where(b => boothIds.Contains(b.BoothId));

            if (CastIds.Any())
                query = query.Where(b => CastIds.Contains(b.CastId));

            if (villageIds.Any())
                query = query.Where(b => b.Villages.Any(v => villageIds.Contains(v.VillageId)));

              

            Expression<Func<Tbl_SeniorDisabled, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Booth.BoothNumber.Equals(Convert.ToInt32(term)) ||
                    b.Name.ToLower().Contains(term) ||
                    b.Address.ToLower().Contains(term) ||
                    b.Cast.CastName.ToLower().Contains(term) ||
                    //b.Village.Id.ToLower().Contains(term) ||
                    b.VoterId.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Booth.BoothNumber,
               projection: m => new SeniorDisabledResponseDto
               {
                   Id = m.Id,
                   TypeId = m.TypeId,
                   TypeName = m.Type.Type,
                   BoothId = m.BoothId,
                   SectorId=m.Booth.SectorId,
                   SectorName=m.Booth.Sector.SectorName,
                   SectorSanyojak=m.Booth.Sector.InchargeName,
                   BoothNumber = m.Booth.BoothNumber,
                   Name = m.Name,
                   Address = m.Address,
                   CategoryId = m.CategoryId,
                   CategoryName = m.Category.Name,
                   CastId = m.CastId,
                   CastName = m.Cast.CastName,
                   Mobile = m.Mobile,
                   VoterId = m.VoterId,
                   Villages = m.Villages.Select(v => new VillageResponseDtos
                   {
                       VillageId = v.VillageId,
                       VillageName = v.Village.VillageName
                   }).ToList(),
               },
               ct:ct
               );
        }
        public async Task<List<seniordisabledExportRow>> GetSeniorDisabledExportAsync(SeniorDisabledQueryParams qp)
        {
            return await _context.Tbl_SeniorDisabled  // replace with your actual table name
                .Where(m => !qp.TypeId.HasValue || m.TypeId == qp.TypeId && m.Status && ( m.UserId == qp.UserId || m.CreatedToUserId == qp.UserId))
                .Select(m => new seniordisabledExportRow
                {
                    BoothNumber = m.Booth != null ? m.Booth.BoothNumber : 0,

                    Village = string.Join(", ", m.Booth.Villages
                        .Select(v => v.Village != null ? v.Village.VillageName : "N/A")),

                    Name = m.Name,
                    MobileNumber = m.Mobile,
                    Category = m.Category != null ? m.Category.Name : "N/A",
                    VoterId = m.VoterId
                })
                .ToListAsync();
        }
        public async Task<Tbl_SeniorDisabled?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_SeniorDisabled
                 .Include(p => p.Villages)
                 .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
