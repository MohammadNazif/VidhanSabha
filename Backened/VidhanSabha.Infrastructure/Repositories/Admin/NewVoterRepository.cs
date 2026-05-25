using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    internal class NewVoterRepository: BaseRepository<Tbl_NewVoter>, INewVoterRepository
    {
        public NewVoterRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<int> AddAsync(Tbl_NewVoter newvoter, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_NewVoter.AddAsync(newvoter);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(Tbl_NewVoter newvoter)
        {
            try
            {
                _context.Tbl_NewVoter.Update(newvoter);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_NewVoter newvoter)
        {
            throw new NotImplementedException();
        }
        public async Task<PagedResult<NewVoterResponseDto>> GetAllAsync(NewVoterQueryParams qp, CancellationToken ct = default)
        {

            var vidhanSabhaId = await _context.Tbl_StatePrabhari
            .Where(u => u.userId == qp.UserId)
            .Select(u => u.VidhansabhaId)
            .FirstOrDefaultAsync();

            var query = _context.Tbl_NewVoter
               .AsNoTracking()
               .AsQueryable();

        

            var boothIds = qp.GetBoothIds();
            var castids = qp.GetCastIds();
            var villageIds = qp.GetVillageIds();

            if (qp.rolefilterflag && (qp.Role == PrabhariRole.BoothSanyojak.ToString() || qp.Role == PrabhariRole.SectorSanyojak.ToString()))
            {
                query = query.Where(f => f.Role == qp.Role.ToString());
            }
            // ✅ FIX 1: query = assign karo, sirf query.Where nahi
            query = query.Where(b =>
                (!qp.Id.HasValue || b.Id == qp.Id) && (b.Booth.Mandal.Status && b.Booth.Sector.Status && b.Booth.Mandal.VidhanId == vidhanSabhaId) &&
                b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId || b.CreatedsectorUserId == qp.UserId);                 // ✅ FIX 2: closing brace sahi jagah

            if (boothIds.Any())
                query = query.Where(b => boothIds.Contains(b.BoothId));

            if (castids.Any())
                query = query.Where(b => castids.Contains(b.CastId));

            if (villageIds.Any())
                query = query.Where(b => b.Villages.Any(v => villageIds.Contains(v.VillageId)));

            Expression<Func<Tbl_NewVoter, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                     b.Booth.BoothNumber.ToString().Contains(term) ||
                     b.Name.ToLower().Contains(term) ||
                     b.FatherName.ToLower().Contains(term) ||
                     b.Category.Name.ToLower().Contains(term) ||
                     b.Cast.CastName.ToLower().Contains(term) ||
                    //b.Village.VillageName.ToLower().Contains(term) ||
                    b.VoterId.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Booth.BoothNumber,
               projection: m => new NewVoterResponseDto
               {
                   Id = m.Id,
                   SectorId=m.Booth.Sector.Id,
                   SectorName=m.Booth.Sector.SectorName,
                   SectorSanyojak=m.Booth.Sector.InchargeName,
                   BoothId = m.BoothId,
                   BoothNumber = m.Booth.BoothNumber,
                   BoothSanyojak=m.Booth.Sanyojak.InchargeName,
                   Name = m.Name,
                   FatherName = m.FatherName,
                   Mobile = m.Mobile,
                   CategoryId = m.CategoryId,
                   CategoryName = m.Category.Name,
                   DOB = m.DOB,
                   Age = m.Age,
                   CastId = m.CastId,
                   CastName = m.Cast.CastName,
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

        public async Task<List<newvoterExportRow>> GetAllForExportAsync(
            NewVoterQueryParams qp,
            CancellationToken ct = default)
        {
            var query = _context.Tbl_NewVoter
                .AsNoTracking()
                .Include(n => n.Booth)
                    .ThenInclude(b => b.Sector)
                .Include(n => n.Booth)
                    .ThenInclude(b => b.Sanyojak)
                .Include(n => n.Category)
                .Include(n => n.Cast)
                .Include(n => n.Villages)
                    .ThenInclude(v => v.Village)
                .AsQueryable();

            // ✅ Filters
            var boothIds = qp.GetBoothIds();
            var castIds = qp.GetCastIds();
            var villageIds = qp.GetVillageIds();

            query = query.Where(b =>
                (!qp.Id.HasValue || b.Id == qp.Id) &&
                b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId || b.CreatedsectorUserId == qp.UserId);

            if (boothIds?.Count > 0)
                query = query.Where(b => boothIds.Contains(b.BoothId));

            if (castIds?.Count > 0)
                query = query.Where(b => castIds.Contains(b.CastId));

            if (villageIds?.Count > 0)
                query = query.Where(b =>
                    b.Villages.Any(v => villageIds.Contains(v.VillageId)));

            // ✅ Search (safe — no Convert crash)
            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim();

                query = query.Where(b =>
                    EF.Functions.Like(b.Name, $"%{term}%") ||
                    EF.Functions.Like(b.FatherName, $"%{term}%") ||
                    EF.Functions.Like(b.Mobile, $"%{term}%") ||
                    EF.Functions.Like(b.VoterId, $"%{term}%") ||
                    EF.Functions.Like(b.Booth.BoothNumber.ToString(), $"%{term}%")
                );
            }

            // ✅ Fetch minimal data
            var data = await query
                .OrderBy(b => b.Booth.BoothNumber)
                .Select(m => new
                {
                    m.Booth.BoothNumber,
                    m.Name,
                    m.FatherName,
                    m.Mobile,
                    Category = m.Category.Name,
                    Cast = m.Cast.CastName,
                    m.DOB,
                    m.Age,
                    m.VoterId,
                    Villages = m.Villages
                        .Select(v => v.Village.VillageName)
                        .ToList()
                })
                .ToListAsync(ct);

            // ✅ Final mapping (string.Join here)
            return data.Select(m => new newvoterExportRow
            {
                BoothNumber = m.BoothNumber,
                Village = string.Join(", ", m.Villages),
                VoterName = m.Name,
                FatherName = m.FatherName,
                MobileNumber = m.Mobile,
                Category = m.Category,
                Cast = m.Cast,
                DOB = m.DOB.ToString("dd-MM-yyyy"),
                Age = m.Age.ToString(),
                VoterId = m.VoterId
            }).ToList();
        }
        public async Task<Tbl_NewVoter?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_NewVoter
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
