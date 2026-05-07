using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class MandalRepository : IMandalRepository
    {
        private readonly DatabaseContext _context;

        public MandalRepository(DatabaseContext context) => _context = context;

        public async Task<PagedResult<MandalResponseDto>> GetAllAsync(MandalQueryParams qp,int? vidhanid,CancellationToken ct=default)
        {
            var query = _context.Tbl_Mandal
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) &&
                    (b.VidhanId == vidhanid)
                   );

            Expression<Func<Tbl_Mandal, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Name.ToLower().Contains(term);
                    //b.Village.VillageName.ToLower().Contains(term) ||
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Id,
               projection: m => new MandalResponseDto
               {
                   Id = m.Id,
                   VidhanId = m.VidhanId,
                   Name = m.Name,
                   Status = m.Status
               },ct:ct);


        }

        public async Task<PagedResult<MandalFullDto>> GetAllCombinedMandalReports(
       MandalQueryParams qp,
       int? vidhansabhaId,
       CancellationToken ct = default)
        {
            var sectorIds = qp.GetSectorIds();
            var mandalIds = qp.GetMandalIds();
            var castIds = qp.GetCastIds();
            var villageIds = qp.GetVillageIds();
            var term = qp.SearchTerm?.Trim().ToLower();

            // =========================
            // 🔹 MAIN MANDAL QUERY
            // =========================
            var query = _context.Tbl_Mandal
                .AsNoTracking()
                .Where(m =>
                    (vidhansabhaId == null || m.VidhanId == vidhansabhaId) &&
                    (!qp.Id.HasValue || m.Id == qp.Id) &&
                    (!mandalIds.Any() || mandalIds.Contains(m.Id)) &&
                    (!sectorIds.Any() || m.Sectors.Any(s => sectorIds.Contains(s.Id))) &&
                    (!castIds.Any() || m.Sectors.Any(s =>
                        s.Booth != null &&
                        s.Booth.Sanyojak != null &&
                        castIds.Contains(s.Booth.Sanyojak.CastId))) &&
                    (!villageIds.Any() || m.Sectors.Any(s =>
                        s.Booth != null &&
                        s.Booth.Villages.Any(v => villageIds.Contains(v.VillageId))))
                );

            // =========================
            // 🔍 SEARCH (MANDAL LEVEL)
            // =========================
            Expression<Func<Tbl_Mandal, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(term))
            {
                search = m =>
                    m.Name.ToLower().Contains(term) ||

                    m.Sectors.Any(s =>
                        s.SectorName.ToLower().Contains(term) ||
                        s.InchargeName.ToLower().Contains(term) ||

                        (s.Booth != null &&
                         s.Booth.PollingStationName.ToLower().Contains(term)) ||

                        (s.Booth != null &&
                         s.Booth.Sanyojak != null &&
                         (
                            s.Booth.Sanyojak.InchargeName.ToLower().Contains(term) ||
                            s.Booth.Sanyojak.PhoneNumber.Contains(term)
                         )) ||

                        (s.Booth != null &&
                         s.Booth.Villages.Any(v =>
                            v.Village != null &&
                            v.Village.VillageName.ToLower().Contains(term)))
                    );
            }

            // =========================
            // 🔥 FILTERED SECTOR COUNT
            // =========================
            var totalFilteredSectors = await _context.Tbl_Sector
                .Where(s =>
                    (vidhansabhaId == null || s.Mandal.VidhanId == vidhansabhaId) &&
                    (!qp.Id.HasValue || s.Mandal.Id == qp.Id) &&
                    (!mandalIds.Any() || mandalIds.Contains(s.Mandal.Id)) &&
                    (!sectorIds.Any() || sectorIds.Contains(s.Id)) &&
                    (!castIds.Any() ||
                        (s.Booth != null &&
                         s.Booth.Sanyojak != null &&
                         castIds.Contains(s.Booth.Sanyojak.CastId))) &&
                    (!villageIds.Any() ||
                        (s.Booth != null &&
                         s.Booth.Villages.Any(v => villageIds.Contains(v.VillageId)))) &&

                    // 🔍 SEARCH (SECTOR LEVEL)
                    (string.IsNullOrWhiteSpace(term) ||

                        s.SectorName.ToLower().Contains(term) ||
                        s.InchargeName.ToLower().Contains(term) ||

                        (s.Booth != null &&
                         s.Booth.PollingStationName.ToLower().Contains(term)) ||

                        (s.Booth != null &&
                         s.Booth.Sanyojak != null &&
                         (
                            s.Booth.Sanyojak.InchargeName.ToLower().Contains(term) ||
                            s.Booth.Sanyojak.PhoneNumber.Contains(term)
                         )) ||

                        (s.Booth != null &&
                         s.Booth.Villages.Any(v =>
                            v.Village != null &&
                            v.Village.VillageName.ToLower().Contains(term)))
                    )
                )
                .CountAsync(ct);

            // =========================
            // 📦 PAGED RESULT
            // =========================
            var result = await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: m => m.Id,
                projection: m => new MandalFullDto
                {
                    Id = m.Id,
                    Name = m.Name,

                    Sectors = m.Sectors
                        .Where(s =>
                            (!sectorIds.Any() || sectorIds.Contains(s.Id)) &&
                            (!castIds.Any() ||
                                (s.Booth != null &&
                                 s.Booth.Sanyojak != null &&
                                 castIds.Contains(s.Booth.Sanyojak.CastId))) &&
                            (!villageIds.Any() ||
                                (s.Booth != null &&
                                 s.Booth.Villages.Any(v => villageIds.Contains(v.VillageId)))) &&

                            // 🔍 SEARCH APPLIED HERE (CRITICAL FIX)
                            (string.IsNullOrWhiteSpace(term) ||

                                s.SectorName.ToLower().Contains(term) ||
                                s.InchargeName.ToLower().Contains(term) ||

                                (s.Booth != null &&
                                 s.Booth.PollingStationName.ToLower().Contains(term)) ||

                                (s.Booth != null &&
                                 s.Booth.Sanyojak != null &&
                                 (
                                    s.Booth.Sanyojak.InchargeName.ToLower().Contains(term) ||
                                    s.Booth.Sanyojak.PhoneNumber.Contains(term)
                                 )) ||

                                (s.Booth != null &&
                                 s.Booth.Villages.Any(v =>
                                    v.Village != null &&
                                    v.Village.VillageName.ToLower().Contains(term)))
                            )
                        )
                        .Select(s => new SectorDto
                        {
                            Id = s.Id,
                            SectorName = s.SectorName,
                            InchargeName = s.InchargeName,
                            PhoneNumber = s.PhoneNumber,

                            Booth = s.Booth != null ? new BoothDto
                            {
                                Id = s.Booth.Id,
                                BoothNumber = s.Booth.BoothNumber,
                                PollingStationName = s.Booth.PollingStationName,

                                Sanyojak = s.Booth.Sanyojak != null
                                    ? new SanyojakDto
                                    {
                                        Name = s.Booth.Sanyojak.InchargeName,
                                        Phone = s.Booth.Sanyojak.PhoneNumber,
                                        FatherName = s.Booth.Sanyojak.FatherName,
                                        Age = s.Booth.Sanyojak.Age,
                                        CastName = s.Booth.Sanyojak.Cast != null
                                            ? s.Booth.Sanyojak.Cast.CastName
                                            : null,
                                        Address = s.Booth.Sanyojak.Address,
                                        Education = s.Booth.Sanyojak.EducationLevel,
                                        ProfilePath = s.Booth.Sanyojak.ProfileImagePath
                                    }
                                    : null,

                                Villages = s.Booth.Villages != null
                                    ? s.Booth.Villages.Select(v => new VillageDto
                                    {
                                        Id = v.VillageId,
                                        Name = v.Village != null
                                            ? v.Village.VillageName
                                            : null
                                    }).ToList()
                                    : new List<VillageDto>()
                            }
                            : null
                        }).ToList()
                },
                ct: ct
            );

            // =========================
            // 🔥 OVERRIDE COUNT
            // =========================
            result.TotalCount = totalFilteredSectors;

            return result;
        }
        public async Task<Tbl_Mandal> GetByIdAsync(int id)
            => await _context.Set<Tbl_Mandal>()
                     .FirstOrDefaultAsync(x => x.Id == id);


        public async Task<bool> ExistsByNameAsync(int? vidhanId, string name)
          => await _context.Set<Tbl_Mandal>().
                            Where(m => m.Status)
                           .AnyAsync(m => m.VidhanId == vidhanId
                                       && m.Name == name.Trim());

        public async Task AddAsync(Tbl_Mandal mandal)
        {
            await _context.Set<Tbl_Mandal>().AddAsync(mandal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tbl_Mandal mandal)
        {
             _context.Set<Tbl_Mandal>().Update(mandal);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetVidhansabhaIdByuserIdAsync(string userId)
        {
            var data = await _context.Tbl_StatePrabhari
                .Where(v => v.userId == userId && v.Status)
                .Select(v => v.VidhansabhaId)
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<PagedResult<MandalReportDto>> GetAllMandalReports(
     MandalQueryParams qp,
     int? vidhanId,
     CancellationToken ct)
        {
            var term = qp.SearchTerm?.Trim().ToLower();
            var mandalIds = qp.GetMandalIds();
            // =========================
            // 🔹 BASE QUERY
            // =========================
            var query = _context.Tbl_Mandal
                .AsNoTracking()
                .Where(m =>
                    m.Status &&
                    (vidhanId == null || m.VidhanId == vidhanId) &&
                    (!qp.Id.HasValue || m.Id == qp.Id)
                );
            if (mandalIds.Any())
            {
                query = query.Where(m => mandalIds.Contains(m.Id));
            }

            // =========================
            // 🔍 SEARCH
            // =========================
            Expression<Func<Tbl_Mandal, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(term))
            {
                search = m =>
                    m.Name.ToLower().Contains(term) ||

                    m.Sectors.Any(s =>
                        s.SectorName.ToLower().Contains(term) ||

                        (s.Booth != null &&
                         s.Booth.PollingStationName.ToLower().Contains(term))
                    );
            }

            // =========================
            // 📦 PAGINATION + PROJECTION
            // =========================
            var result = await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: m => m.Id,
                projection: m => new MandalReportDto
                {
                    MandalId = m.Id,
                    MandalName = m.Name,

                    // 🔥 TOTAL SECTORS
                    TotalSectors = m.Sectors.Count(s => s.Status && s.CreatedByUserId == qp.UserId),

                    // 🔥 TOTAL BOOTHS (FIXED: no SelectMany)
                    TotalBooths = _context.Tbl_Booth
                        .Count(b =>
                            b.Status &&
                            b.MandalId == m.Id && b.UserId == qp.UserId),

                    // 🔥 TOTAL VOTES (SUM)
                    TotalVotes = _context.Tbl_BoothVoter
                        .Where(v =>
                            v.Status && v.UserId == qp.UserId && v.Booth.Mandal.Status &&
                            v.Booth != null &&
                            v.Booth.MandalId == m.Id)
                        .Sum(v => (int?)v.TotalVoter) ?? 0,

                    // 🔥 SENIOR CITIZEN
                    SeniorCitizen = _context.Tbl_SeniorDisabled
                        .Count(x =>
                            x.Status && x.UserId == qp.UserId && x.Type.Status && x.Booth.Mandal.Status &&
                            x.TypeId == 1 &&
                            x.Booth != null &&
                            x.Booth.MandalId == m.Id),

                    // 🔥 HANDICAP
                    Handicap = _context.Tbl_SeniorDisabled
                        .Count(x =>
                            x.Status && x.UserId == qp.UserId && x.Type.Status && x.Booth.Mandal.Status &&
                            x.TypeId == 2 &&
                            x.Booth != null &&
                            x.Booth.MandalId == m.Id),

                    // 🔥 DOUBLE VOTERS
                    DoubleVotes = _context.Tbl_DoubleVoter
                        .Count(x =>
                            x.Status && x.UserId == qp.UserId && x.Booth.Mandal.Status &&
                            x.Booth != null &&
                            x.Booth.MandalId == m.Id),

                    // 🔥 PRAVASI
                    Pravasi = _context.Tbl_PravasiVoter
                        .Count(x =>
                            x.Status && x.UserId == qp.UserId && x.Booth.Mandal.Status &&
                            x.Booth != null &&
                            x.Booth.MandalId == m.Id),

                    // 🔥 EFFECTIVE PERSON
                    EffectivePerson = _context.Tbl_PrabhavshaliVyakti
                    .Count(x => x.Status && x.UserId == qp.UserId && x.Booth.Mandal.Status && x.Designation.Status &&
                            x.Booth != null &&
                            x.Booth.MandalId == m.Id)
                       
                },
                ct: ct
            );

            return result;
        }
    }
}