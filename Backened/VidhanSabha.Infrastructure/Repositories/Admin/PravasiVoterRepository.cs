using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class PravasiVoterRepository:BaseRepository<Tbl_PravasiVoter>,IPravasiVoterRepository
    {
        public PravasiVoterRepository(DatabaseContext context):base(context)
        {

        }
        public async Task<int> AddAsync(Tbl_PravasiVoter pravasi,CancellationToken ct=default)
        {
            try
            {
                await _context.Tbl_PravasiVoter.AddAsync(pravasi);
                return await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
        }

        public int Update(Tbl_PravasiVoter pravasi)
        {
            try
            {
                _context.Tbl_PravasiVoter.Update(pravasi);
                return _context.SaveChanges();
            }
            catch(Exception)
            {
                throw;
            }
            
        }
        public void Delete(Tbl_PravasiVoter pravasi)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<PravasiVoterResponseDto>> GetAllAsync(PravasiQueryParams qp,CancellationToken ct=default)
        {
            var query = _context.Tbl_PravasiVoter
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) &&
                   (!qp.BoothId.HasValue || b.Booth.Id == qp.BoothId) &&
                   (!qp.CastId.HasValue || b.Cast.Id == qp.CastId) &&
                   (!qp.OccupationId.HasValue || b.OccupationId == qp.OccupationId)
                   );

            Expression<Func<Tbl_PravasiVoter, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Booth.BoothNumber.Equals(Convert.ToInt32(term)) ||
                    b.Name.ToLower().Contains(term) ||
                    b.Cast.CastName.ToLower().Contains(term) ||
                    //b.Village.VillageName.ToLower().Contains(term) ||
                    b.VoterId.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Booth.BoothNumber,
               projection: m => new PravasiVoterResponseDto
               {
                   Id = m.Id,
                   BoothId = m.BoothId,
                   BoothNumber = m.Booth.BoothNumber,
                   Name = m.Name,
                   Mobile = m.Mobile,
                   CategoryId = m.CategoryId,
                   CategoryName = m.Category.Name,
                   CastId = m.CastId,
                   CastName = m.Cast.CastName,
                   OccupationId = m.OccupationId,
                   Occupation = m.Occupation.Occupation,
                   VoterId = m.VoterId,
                   CurrentAddress = m.CurrentAddress,
                   Villages = m.Villages.Select(v => new VillageResponseDto
                   {
                       VillageId = v.VillageId,
                       VillageName = v.Village.VillageName
                   }).ToList(),
               },
               ct:ct
               );
        }

        public async Task<Tbl_PravasiVoter?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_PravasiVoter
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
