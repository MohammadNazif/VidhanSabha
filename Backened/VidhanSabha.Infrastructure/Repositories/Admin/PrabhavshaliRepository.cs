using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class PrabhavshaliRepository:BaseRepository<Tbl_PrabhavshaliVyakti>,IPrabhavshaliRepository
    {
        public PrabhavshaliRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<int> AddAsync(Tbl_PrabhavshaliVyakti prabhav, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_PrabhavshaliVyakti.AddAsync(prabhav);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }


        }
        public int Update(Tbl_PrabhavshaliVyakti prabhav)
        {
            try
            {
                _context.Tbl_PrabhavshaliVyakti.Update(prabhav);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_PrabhavshaliVyakti prabhav)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<PrabhavshaliResponseDto>> GetAllAsync(PrabhavshaliQueryParams qp, CancellationToken ct = default)
        {
            var query = _context.Tbl_PrabhavshaliVyakti
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) &&
                   (!qp.BoothId.HasValue || b.Booth.Id == qp.BoothId) &&
                   (!qp.VillageId.HasValue || b.Villages.Any(v => v.VillageId == qp.VillageId) &&
                   (!qp.CastId.HasValue || b.Cast.Id == qp.CastId))

                   );

            Expression<Func<Tbl_PrabhavshaliVyakti, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Booth.BoothNumber.Equals(Convert.ToInt32(term)) ||
                    b.Name.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Booth.BoothNumber,
               projection: m => new PrabhavshaliResponseDto
               {
                   Id = m.Id,
                   BoothId = m.BoothId,
                   BoothNumber = m.Booth.BoothNumber,
                   DesignationId = m.DesignationId,
                   DesignationName = m.Designation.DesignationName,
                   Name = m.Name,
                   CategoryId = m.CategoryId,
                   CategoryName = m.Category.Name,
                   CastId = m.CastId,
                   CastName = m.Cast.CastName,
                   Mobile = m.Mobile,
                   Description = m.Description,
                   Villages = m.Villages.Select(v => new VillageResponseDtos
                   {
                       VillageId = v.VillageId,
                       VillageName = v.Village.VillageName
                   }).ToList(),
               },ct:ct);
        }


        public async Task<Tbl_PrabhavshaliVyakti?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_PrabhavshaliVyakti
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
