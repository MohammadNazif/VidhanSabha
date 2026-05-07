using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
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

         //   var vidhanSabhaId = await _context.Tbl_StatePrabhari
         //.Where(u => u.userId == qp.UserId)
         //.Select(u => u.VidhansabhaId)
         //.FirstOrDefaultAsync();

            var query = _context.Tbl_SahmatAsahmat
              .AsNoTracking();

            var villageIds = qp.GetVillageIds();
            var boothIds = qp.GetBoothIds();
            var parties = qp.GetParties();
              
              if (villageIds.Any())
              {
                  query = query.Where(s => s.Villages.Any(v => villageIds.Contains(v.VillageId)));
            }
                if (parties.Any())
                {
                    query = query.Where(s => parties.Contains(s.PartyId));
            }
                if (boothIds.Any())
                  {
                    query = query.Where(s => boothIds.Contains(s.BoothId));
            }

            query = query.Where(b =>
                   (!qp.TypeId.HasValue || b.TypeId == qp.TypeId) && (b.Booth.Mandal.Status && b.Booth.Sector.Status ) && 
                   (b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId || b.CreatedsectorUserId == qp.UserId) &&
                  (!qp.BoothId.HasValue || b.Booth.Id == qp.BoothId) &&
                  (!qp.OccupationId.HasValue || b.Occupation.Id == qp.OccupationId)
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
        public async Task<List<sahmatExportRow>> GetAllForExportAsync(
       SahmatAsahmatQueryParams qp,
        CancellationToken ct = default)
        {
            var query = _context.Tbl_SahmatAsahmat
                .AsNoTracking()
                .Include(s => s.Booth)
                .Include(s => s.Party)
                .Include(s => s.Occupation)
                .Include(s => s.Type)
                .Include(s => s.Villages)
                    .ThenInclude(v => v.Village)
                .AsQueryable();

            // ✅ Filters
            query = query.Where(b =>
                (!qp.BoothId.HasValue || b.BoothId == qp.BoothId) &&
                (b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId || b.CreatedsectorUserId == qp.UserId) &&
               
                (!qp.OccupationId.HasValue || b.OccupationId == qp.OccupationId)
            );

            // ✅ 🔥 MAIN FILTER (Sahmat / Asahmat)
            if (!string.IsNullOrWhiteSpace(qp.Type))
            {
                query = query.Where(b => b.Type.Type == qp.Type);
            }

            // ✅ Search
            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim();

                query = query.Where(b =>
                    EF.Functions.Like(b.Name, $"%{term}%") ||
                    EF.Functions.Like(b.Mobile, $"%{term}%") ||
                    EF.Functions.Like(b.VoterId, $"%{term}%") ||
                    EF.Functions.Like(b.Booth.BoothNumber.ToString(), $"%{term}%")
                );
            }

            var data = await query
                .OrderBy(b => b.Booth.BoothNumber)
                .Select(m => new
                {
                    m.Booth.BoothNumber,
                    m.Name,
                    m.Mobile,
                    m.Age,
                    Party = m.Party.Party,
                    Occupation = m.Occupation.Occupation,
                    m.VoterId,
                    Status = m.Type.Type,
                    Villages = m.Villages.Select(v => v.Village.VillageName).ToList()
                })
                .ToListAsync(ct);

            return data.Select(m => new sahmatExportRow
            {
                BoothNumber = m.BoothNumber,
                Village = string.Join(", ", m.Villages),
                Name = m.Name,
                MobileNumber = m.Mobile,
                Age = m.Age.ToString(),
                Party = m.Party,
                Occupation = m.Occupation,
                VoterId = m.VoterId,
                Status = m.Status
            }).ToList();
        }

    }
}
