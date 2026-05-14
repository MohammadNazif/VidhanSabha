using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos.VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
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
                b.Name.ToLower().Contains(term) ||
                b.Sanyojak.InchargeName.ToLower().Contains(term)||
                b.Sanyojak.FatherName.ToLower().Contains(term) ||
                b.Sanyojak.Cast.CastName.ToLower().Contains(term);
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
                   IsMandalSanyojak = m.IsMandalSanyojak,
                   MandalSanyojak =m.Sanyojak.InchargeName,
                   CastId = m.Sanyojak.CastId,
                   CastName = m.Sanyojak.Cast.CastName,
                   CategoryId = m.Sanyojak.CategoryId,
                   Contact = m.Sanyojak.Contact,
                   FatherName = m.Sanyojak.FatherName,
                   Age = m.Sanyojak.Age,
                   Address =m.Sanyojak.Address,
                   Education = m.Sanyojak.EducationLevel,
                   Profile = m.Sanyojak.ProfileImagePath,
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

            // =====================================================
            // BASE QUERY
            // =====================================================
            var query = _context.Tbl_Sector
                .AsNoTracking()
                .Where(s =>
                    s.SectorName != null &&
                    s.SectorName != "" &&

                    (vidhansabhaId == null || s.Mandal.VidhanId == vidhansabhaId) &&
                    (!qp.Id.HasValue || s.Mandal.Id == qp.Id) &&
                    (!mandalIds.Any() || mandalIds.Contains(s.Mandal.Id)) &&
                    (!sectorIds.Any() || sectorIds.Contains(s.Id)) &&
                    (!castIds.Any() || (s.Booth != null && s.Booth.Sanyojak != null && castIds.Contains(s.Booth.Sanyojak.CastId))) &&
                    (!villageIds.Any() || (s.Booth != null && s.Booth.Villages.Any(v => villageIds.Contains(v.VillageId)))) &&

                    (string.IsNullOrWhiteSpace(term) ||
                        (s.SectorName != null && s.SectorName.ToLower().Contains(term)) ||
                        (s.InchargeName != null && s.InchargeName.ToLower().Contains(term)) ||
                        (s.FatherName != null && s.FatherName.ToLower().Contains(term)) ||
                        (s.Booth != null && s.Booth.PollingStationName != null && s.Booth.PollingStationName.ToLower().Contains(term)) ||
                        (s.Booth != null && s.Booth.Sanyojak != null &&
                            ((s.Booth.Sanyojak.InchargeName != null && s.Booth.Sanyojak.InchargeName.ToLower().Contains(term)) ||
                             (s.Booth.Sanyojak.PhoneNumber != null && s.Booth.Sanyojak.PhoneNumber.Contains(term)))) ||
                        (s.Booth != null && s.Booth.Villages.Any(v => v.Village != null && v.Village.VillageName != null && v.Village.VillageName.ToLower().Contains(term)))
                    )
                )

                // =====================================================
                // SINGLE ROW DTO
                // =====================================================
                .Select(s => new MandalFullDto
                {
                    // Mandal
                    MandalId = s.Mandal.Id,
                    MandalName = s.Mandal.Name,

                    // Sector
                    SectorId = s.Id,
                    SectorName = s.SectorName,
                    SectorPhone = s.PhoneNumber,
                    SectorInchargeName = s.InchargeName,
                    SectorFatherName = s.FatherName,

                    // Booth
                    BoothId = s.Booth != null ? s.Booth.Id : null,
                    BoothNumber = s.Booth != null ? s.Booth.BoothNumber : null,
                    PollingStationName = s.Booth != null ? s.Booth.PollingStationName : null,

                    // Sanyojak
                    SanyojakName = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.InchargeName : null,
                    SanyojakPhone = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.PhoneNumber : null,
                    SanyojakFatherName = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.FatherName : null,
                    SanyojakAge = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.Age : null,
                    SanyojakCaste = s.Booth != null && s.Booth.Sanyojak != null && s.Booth.Sanyojak.Cast != null ? s.Booth.Sanyojak.Cast.CastName : null,
                    SanyojakAddress = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.Address : null,
                    SanyojakEducation = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.EducationLevel : null,
                    SanyojakProfile = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.ProfileImagePath : null,

                    // Villages (comma-separated)
                    VillageNames = s.Booth != null
                        ? string.Join(", ",
                            s.Booth.Villages
                                .Where(v => v.Village != null)
                                .Select(v => v.Village.VillageName))
                        : null
                });

            // =====================================================
            // TOTAL COUNT (matches projected rows)
            // =====================================================
            var totalCount = await query.CountAsync(ct);

            // =====================================================
            // PAGINATION
            // =====================================================
            var items = await query
                .OrderBy(x => x.MandalId)
                .ThenBy(x => x.SectorId)
                .Skip((qp.PageNumber - 1) * qp.PageSize)
                .Take(qp.PageSize)
                .ToListAsync(ct);

            // =====================================================
            // RESULT
            // =====================================================
            return new PagedResult<MandalFullDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = qp.PageNumber,
                PageSize = qp.PageSize
            };
        }
        public async Task<List<CombinedReportExportRow>> GetAllCombinedMandalReportsExp(
       CombinedReportFilter qp,
       int? vidhansabhaId,
       CancellationToken ct = default)
        {
            var sectorIds = qp.GetSectorIds();
            var mandalIds = qp.GetMandalIds();
            var castIds = qp.GetCastIds();
            var villageIds = qp.GetVillageIds();
            var term = qp.SearchTerm?.Trim().ToLower();

            var query = _context.Tbl_Sector
                .AsNoTracking()
                .Where(s =>
                    !string.IsNullOrWhiteSpace(s.SectorName) &&
                    (vidhansabhaId == null || s.Mandal.VidhanId == vidhansabhaId) &&
                    (!qp.Id.HasValue || s.Mandal.Id == qp.Id) &&
                    (!mandalIds.Any() || mandalIds.Contains(s.Mandal.Id)) &&
                    (!sectorIds.Any() || sectorIds.Contains(s.Id)) &&
                    (!castIds.Any() || (s.Booth != null && s.Booth.Sanyojak != null && castIds.Contains(s.Booth.Sanyojak.CastId))) &&
                    (!villageIds.Any() || (s.Booth != null && s.Booth.Villages.Any(v => villageIds.Contains(v.VillageId)))) &&
                    (string.IsNullOrWhiteSpace(term) ||
                        (s.SectorName != null && s.SectorName.ToLower().Contains(term)) ||
                        (s.InchargeName != null && s.InchargeName.ToLower().Contains(term)) ||
                        (s.FatherName != null && s.FatherName.ToLower().Contains(term)) ||
                        (s.Booth != null && s.Booth.PollingStationName != null && s.Booth.PollingStationName.ToLower().Contains(term)) ||
                        (s.Booth != null && s.Booth.Sanyojak != null &&
                            ((s.Booth.Sanyojak.InchargeName != null && s.Booth.Sanyojak.InchargeName.ToLower().Contains(term)) ||
                             (s.Booth.Sanyojak.PhoneNumber != null && s.Booth.Sanyojak.PhoneNumber.Contains(term)))) ||
                        (s.Booth != null && s.Booth.Villages.Any(v => v.Village != null && v.Village.VillageName != null && v.Village.VillageName.ToLower().Contains(term)))
                    )
                )
                .Select(s => new CombinedReportExportRow
                {
                    MandalId = s.Mandal.Id,
                    MandalName = s.Mandal.Name,
                   
                    SectorName = s.SectorName,
                    SectorPhone = s.PhoneNumber,
                    SectorInchargeName = s.InchargeName,
                    SectorFatherName = s.FatherName,
                
                    BoothNumber = s.Booth != null ? s.Booth.BoothNumber : null,
                    PollingStationName = s.Booth != null ? s.Booth.PollingStationName : null,
                    SanyojakName = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.InchargeName : null,
                    SanyojakPhone = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.PhoneNumber : null,
                    SanyojakFatherName = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.FatherName : null,
                    SanyojakAge = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.Age : null,
                    SanyojakCaste = s.Booth != null && s.Booth.Sanyojak != null && s.Booth.Sanyojak.Cast != null ? s.Booth.Sanyojak.Cast.CastName : null,
                    SanyojakAddress = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.Address : null,
                    SanyojakEducation = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.EducationLevel : null,
                    SanyojakProfile = s.Booth != null && s.Booth.Sanyojak != null ? s.Booth.Sanyojak.ProfileImagePath : null,
                    VillageNames = s.Booth != null
                        ? string.Join(", ", s.Booth.Villages.Where(v => v.Village != null).Select(v => v.Village.VillageName))
                        : null
                });

            return await query.OrderBy(r => r.MandalId).ToListAsync(ct);
        }

        public async Task<Tbl_Mandal> GetByIdAsync(int id)
         => await _context.Set<Tbl_Mandal>()
                  .Include(x => x.Sanyojak)   // ← add this
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

        public async Task<List<MandalExportRow>> GetMandalExportAsync(
    MandalFilter filter,
    int? vidhanId,
    CancellationToken ct = default)
        {
            var query = _context.Tbl_Mandal
                .AsNoTracking()
                .Where(m =>
                    (!filter.Id.HasValue || m.Id == filter.Id) &&
                    (vidhanId == null || m.VidhanId == vidhanId)
                );

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim().ToLower();
                query = query.Where(m => m.Name.ToLower().Contains(term));
            }

            var result = await query
                .Select(m => new MandalExportRow
                {
                   
                    Name = m.Name,
                    MandalSanyojak = m.Sanyojak != null ? m.Sanyojak.InchargeName : null,
                    CastName = m.Sanyojak != null && m.Sanyojak.Cast != null ? m.Sanyojak.Cast.CastName : null,
                    Contact = m.Sanyojak != null ? m.Sanyojak.Contact : null,
                    FatherName = m.Sanyojak != null ? m.Sanyojak.FatherName : null,

                })
                .ToListAsync(ct);

            return result;
        }
        public async Task<List<MandalReportExportRow>> GetAllMandalReportsForExport(
       MandalQueryParams qp,
    int? vidhanId)
        {
            var mandalIds = qp.GetMandalIds();

            var query = _context.Tbl_Mandal
                .AsNoTracking()
                .Where(m =>
                    m.Status &&
                    (vidhanId == null || m.VidhanId == vidhanId) &&
                    (!qp.Id.HasValue || m.Id == qp.Id)
                );

            if (mandalIds.Any())
                query = query.Where(m => mandalIds.Contains(m.Id));

            var result = await query
                .Select(m => new MandalReportExportRow
                {
                    MandalId = m.Id,
                    MandalName = m.Name,

                    TotalSectors = m.Sectors.Count(s => s.Status && s.CreatedByUserId == qp.UserId),

                    TotalBooths = _context.Tbl_Booth.Count(b => b.Status && b.MandalId == m.Id && b.UserId == qp.UserId),

                    TotalVotes = _context.Tbl_BoothVoter
                        .Where(v => v.Status && v.UserId == qp.UserId && v.Booth.Mandal.Status &&
                                    v.Booth != null && v.Booth.MandalId == m.Id)
                        .Sum(v => (int?)v.TotalVoter) ?? 0,

                    SeniorCitizen = _context.Tbl_SeniorDisabled
                        .Count(x => x.Status && x.UserId == qp.UserId && x.Type.Status && x.Booth.Mandal.Status &&
                                    x.TypeId == 1 && x.Booth != null && x.Booth.MandalId == m.Id),

                    Handicap = _context.Tbl_SeniorDisabled
                        .Count(x => x.Status && x.UserId == qp.UserId && x.Type.Status && x.Booth.Mandal.Status &&
                                    x.TypeId == 2 && x.Booth != null && x.Booth.MandalId == m.Id),

                    DoubleVotes = _context.Tbl_DoubleVoter
                        .Count(x => x.Status && x.UserId == qp.UserId && x.Booth.Mandal.Status &&
                                    x.Booth != null && x.Booth.MandalId == m.Id),

                    Pravasi = _context.Tbl_PravasiVoter
                        .Count(x => x.Status && x.UserId == qp.UserId && x.Booth.Mandal.Status &&
                                    x.Booth != null && x.Booth.MandalId == m.Id),

                    EffectivePerson = _context.Tbl_PrabhavshaliVyakti
                        .Count(x => x.Status && x.UserId == qp.UserId && x.Booth.Mandal.Status && x.Designation.Status &&
                                    x.Booth != null && x.Booth.MandalId == m.Id)
                })
                .ToListAsync();

            return result;
        }
        public async Task<MandalSanyojakDto> GetMandalSanyojakByIdAsync(int id)
        {
            var data = await _context.Tbl_MandalSanyojak
                .Include(x => x.Mandal) // ensure Mandal navigation property is loaded
                .Where(x => x.MandalId == id) // check Status properly
                .Select(x => new MandalSanyojakDto
                {
                    Id = x.Id,
                    MandalName = x.Mandal.Name,
                    InchargeName = x.InchargeName,
                    //Contact = x.Contact,
                    //TotalMember = _context.Tbl_MandalSanyojakMember
                    //    .Count(m => m.MandalSanyojakId == x.Id && m.Status == 1)
                })
                .FirstOrDefaultAsync();

            return data;
        }
    }
}