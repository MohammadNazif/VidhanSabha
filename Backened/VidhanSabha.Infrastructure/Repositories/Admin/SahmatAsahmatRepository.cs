using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class SahmatAsahmatRepository:BaseRepository<Tbl_SahmatAsahmat>,ISahmatAsahmatRepository
    {
        public SahmatAsahmatRepository(DatabaseContext context) : base(context) 
        {

        }
        public async Task<int> AddAsync(Tbl_SahmatAsahmat sahmatasahmat, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_SahmatAsahmat.AddAsync(sahmatasahmat);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Tbl_SahmatAsahmat sahmatasahmat)
        {
            try
            {
                _context.Tbl_SahmatAsahmat.Update(sahmatasahmat);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_SahmatAsahmat sahmatasahmat)
        {
            throw new NotImplementedException();
        }


        public async Task<Tbl_SahmatAsahmat?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_SahmatAsahmat
                 .Include(p => p.Villages)

                 .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagedResult<SahmatAsahmatResponseDto>> GetAllAsync(SahmatAsahmatQueryParams qp, CancellationToken ct = default)
        {
            var query = _context.Tbl_SahmatAsahmat
              .AsNoTracking()
              .Where(b =>
                  (!qp.Id.HasValue || b.Id == qp.Id) &&
                  (!qp.BoothId.HasValue || b.Booth.Id == qp.BoothId) &&
                  (!qp.VillageId.HasValue || b.Villages.Any(v => v.VillageId == qp.VillageId) &&
                  (!qp.OccupationId.HasValue || b.Occupation.Id == qp.OccupationId))

                  );

            Expression<Func<Tbl_SahmatAsahmat, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Booth.BoothNumber.Equals(Convert.ToInt32(term)) ||
                    b.Name.ToLower().Contains(term) ||
                    b.Occupation.Occupation.ToLower().Contains(term) ||
                    b.Party.Party.ToLower().Contains(term) ||
                    //b.Village.VillageName.ToLower().Contains(term) ||
                    b.VoterId.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Booth.BoothNumber,
               projection: m => new SahmatAsahmatResponseDto
               {
                   Id = m.Id,
                   BoothId = m.BoothId,
                   BoothNumber = m.Booth.BoothNumber,
                   TypeId = m.TypeId,
                   Type = m.Type.Type,
                   Name = m.Name,
                   Age = m.Age,
                   Mobile = m.Mobile,
                   PartyId = m.PartyId,
                   Party = m.Party.Party,
                   OccupationId = m.OccupationId,
                   Occupation = m.Occupation.Occupation,
                   Reason = m.Reason ?? "NA",
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

    }
}
