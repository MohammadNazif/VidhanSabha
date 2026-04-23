using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
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
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) &&
                   (!qp.BoothId.HasValue || b.Booth.Id == qp.BoothId) &&
                   (!qp.VillageId.HasValue || b.Villages.Any(v => v.VillageId == qp.VillageId) &&
                   (!qp.CastId.HasValue || b.Cast.Id == qp.CastId))

                   );

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
