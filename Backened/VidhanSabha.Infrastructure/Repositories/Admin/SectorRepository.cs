using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class SectorRepository : BaseRepository<Tbl_Sector>, ISectorRepository
    {
        public SectorRepository(DatabaseContext context) : base(context) { }

        public async Task<PagedResult<SectorResponseDto>> GetAllAsync(SectorQueryParams qp, int? vidhanId, CancellationToken ct)
        {
            var query = _context.Tbl_Sector
                .AsNoTracking()
                .Where(x => x.Mandal.VidhanId == vidhanId)
                .Where(b => (!qp.Id.HasValue || b.Id == qp.Id));

            Expression<Func<Tbl_Sector, bool>>? search = null;
            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b => b.SectorName.ToLower().Contains(term) ||
                   b.Mandal.Name.ToLower().Contains(term) ||
                   b.InchargeName.ToLower().Contains(term) 
                ;
            }

            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: b => b.Id,
                projection: s => new SectorResponseDto
                {
                    Id = s.Id,
                    MandalId = s.MandalId,
                    MandalName = s.Mandal != null ? s.Mandal.Name : null,
                    Villages = s.Villages.Select(v => new VillageResponseDto
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village != null ? v.Village.VillageName : null  // ✅ null safe
                    }).ToList(),
                    SectorName = s.SectorName,
                    IsSectorSanyojak = s.IsSectorSanyojak,
                    InchargeName = s.InchargeName,
                    Age = s.Age,
                    FatherName = s.FatherName,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category != null ? s.Category.Name : null,      
                    CastId = s.CastId,
                    CastName = s.Cast != null ? s.Cast.CastName : null,
                    EducationLevel = s.EducationLevel,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    Profile = s.ProfileImage,
                    Status = s.Status
                },
                ct: ct
            );
        }
        public async Task<PagedResult<SectorReportDto>> GetAllSectorReports(
        SectorQueryParams qp,
        int? vidhanSabhaId,
        CancellationToken ct = default)
        {
            var term = qp.SearchTerm?.Trim().ToLower();

            // =========================
            // 🔹 BASE QUERY
            // =========================
            var query = _context.Tbl_Sector
                .AsNoTracking()
                .Where(s =>
                    s.Status &&
                    s.CreatedByUserId == qp.UserId &&
                    (vidhanSabhaId == null || s.Mandal.VidhanId == vidhanSabhaId) &&
                    (!qp.MandalId.HasValue || s.MandalId == qp.MandalId) &&
                    (!qp.SectorId.HasValue || s.Id == qp.SectorId) &&
                    (!qp.BoothId.HasValue || s.Booth.Id == qp.BoothId) &&
                    (!qp.VillageId.HasValue ||
                        s.Villages.Any(v => v.VillageId == qp.VillageId) ||
                        (s.Booth != null && s.Booth.Villages.Any(v => v.VillageId == qp.VillageId)))
                );

            // ✅ CastId filter separate to avoid null cast rows being dropped
            if (qp.CastId.HasValue)
                query = query.Where(s => s.CastId == qp.CastId.Value);


            var mandalIds = qp.GetMandalIds();
            var villageIds = qp.GetVillageIds();
            var castIds = qp.GetCastIds();
            var sectorIds = qp.GetSectorIds();

            if (mandalIds.Any())
            {
                query = query.Where(b => mandalIds.Contains(b.MandalId));
            }

            if (villageIds.Any())
            {
                query = query.Where(b => b.Villages.Any(v => villageIds.Contains(v.VillageId)));
            }

            if (castIds.Any())
            {
                query = query.Where(b => castIds.Contains(b.CastId));
            }
            if (sectorIds.Any())
            {
                query = query.Where(b => sectorIds.Contains(b.Id));
            }
            // =========================
            // 🔍 SEARCH — baked into query directly, null-safe
            // =========================
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(s =>
                    (s.SectorName != null && s.SectorName.ToLower().Contains(term)) ||
                    (s.InchargeName != null && s.InchargeName.ToLower().Contains(term)) ||
                    (s.PhoneNumber != null && s.PhoneNumber.Contains(term)) ||

                    (s.Mandal != null &&
                     s.Mandal.Name != null &&
                     s.Mandal.Name.ToLower().Contains(term)) ||

                    (s.Booth != null &&
                     s.Booth.PollingStationName != null &&
                     s.Booth.PollingStationName.ToLower().Contains(term)) ||

                    (s.Booth != null &&
                     s.Booth.Sanyojak != null &&
                     s.Booth.Sanyojak.InchargeName != null &&
                     s.Booth.Sanyojak.InchargeName.ToLower().Contains(term)) ||

                    (s.Booth != null &&
                     s.Booth.Sanyojak != null &&
                     s.Booth.Sanyojak.PhoneNumber != null &&
                     s.Booth.Sanyojak.PhoneNumber.Contains(term)) ||

                    (s.Booth != null &&
                     s.Booth.Villages.Any(v =>
                        v.Village != null &&
                        v.Village.VillageName != null &&
                        v.Village.VillageName.ToLower().Contains(term)))
                );
            }

            // =========================
            // 📦 PAGINATION + PROJECTION
            // =========================
            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: null, // ✅ already applied above
                defaultSort: s => s.Id,
                projection: s => new SectorReportDto
                {
                    MandalId = s.Mandal.Id,
                    MandalName = s.Mandal.Name,

                    SectorId = s.Id,
                    SectorName = s.SectorName,
                    InchargeName = s.InchargeName,
                    Age = s.Age,
                    FatherName = s.FatherName,
                    CastId = s.CastId != null ? (s.CastId != null ? s.CastId : null) : null,
                    //CastName = s.CastId != null ? (s.Cast != null ? s.Cast.CastName : null) : null,
                    EducationLevel = s.EducationLevel,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    ProfileImage = s.ProfileImage,

                    SectorVillages = s.Villages != null
                        ? s.Villages.Select(v => new VillageDto
                        {
                            Id = v.VillageId,
                            Name = v.Village != null ? v.Village.VillageName : null
                        }).ToList()
                        : new List<VillageDto>(),

                    Booth = s.Booth != null
                        ? new BoothDto
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
                                    Name = v.Village != null ? v.Village.VillageName : null
                                }).ToList()
                                : new List<VillageDto>()
                        }
                        : null
                },
                ct: ct
            );
        }
        public async Task<PagedResult<AdminSectorReportsDto>> GetAllAdminSectorReports(
     SectorQueryParams qp,
     CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(qp.UserId))
                throw new ArgumentException("UserId is required for sector reports.");

            var term = qp.SearchTerm?.Trim().ToLower();

            var mandalIds = qp.GetMandalIds();
            var villageIds = qp.GetVillageIds();
            var castIds = qp.GetCastIds();
            var SectorIds = qp.GetSectorIds();

            // =========================
            // 🔹 BASE QUERY
            // =========================
            var query = _context.Tbl_Sector
                .AsNoTracking()
                .Where(s =>
                    s.CreatedByUserId == qp.UserId &&
                    (!qp.SectorId.HasValue || s.Id == qp.SectorId)
                );

            // ✅ Only filter by CastId if explicitly provided — don't touch null CastId rows
            if (qp.CastId.HasValue)
                query = query.Where(s => s.CastId == qp.CastId.Value);

            if (mandalIds.Any())
                query = query.Where(s => mandalIds.Contains(s.MandalId));

            if (villageIds.Any())
                query = query.Where(s => s.Villages.Any(v => villageIds.Contains(v.VillageId)));

            if (castIds.Any())
                query = query.Where(s => s.CastId != null && castIds.Contains(s.CastId));


            //if (sect.Any())
            //    query = query.Where(s => s.CastId != null && castIds.Contains(s.CastId));

            // =========================
            // 🔍 SEARCH — null-safe
            // =========================
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(s =>
                    (s.SectorName != null && s.SectorName.ToLower().Contains(term)) ||
                    (s.InchargeName != null && s.InchargeName.ToLower().Contains(term)) ||
                    (s.PhoneNumber != null && s.PhoneNumber.Contains(term)));
            }

            // =========================
            // 📦 PAGINATION + DATA
            // =========================
            return await query.ToPagedResultAsync(
      queryParams: qp,
      searchPredicate: null,
      defaultSort: s => s.Id,
      projection: s => new AdminSectorReportsDto
      {
          SectorId = s.Id,
          SectorName = s.SectorName ?? string.Empty,
          SectorSanyojak = s.IsSectorSanyojak ? (s.InchargeName ?? string.Empty) : string.Empty,
          Mobile = s.IsSectorSanyojak ? (s.PhoneNumber ?? string.Empty) : string.Empty,
          Cast = s.Cast != null ? s.Cast.CastName : null,

          Villages = s.Villages != null
              ? s.Villages.Select(v => new VillageDto
              {
                  Id = v.VillageId,
                  Name = v.Village != null ? v.Village.VillageName : null
              }).ToList()
              : new List<VillageDto>(),

          TotalBooth = _context.Tbl_Booth
              .Count(x =>
                  x.SectorId == s.Id &&
                  x.Status),

          TotaVotes = _context.Tbl_BoothVoter
              .Where(x =>
                  x.Booth.SectorId == s.Id &&
                  x.Booth.Sector.CreatedByUserId == qp.UserId && 
                  x.Status)
              .Sum(x => (int?)x.TotalVoter) ?? 0,

          SeniorCitizen = _context.Tbl_SeniorDisabled
              .Count(x =>
                  x.Booth.SectorId == s.Id &&
                  x.Booth.Sector.CreatedByUserId == qp.UserId && x.UserId == qp.UserId &&
                  x.TypeId == 1 &&
                  x.Status),

          Handicap = _context.Tbl_SeniorDisabled
              .Count(x =>
                  x.Booth.SectorId == s.Id &&
                  x.Booth.Sector.CreatedByUserId == qp.UserId  && x.UserId == qp.UserId &&
                  x.TypeId == 2 &&
                  x.Status),

          DoubleVoter = _context.Tbl_DoubleVoter
              .Count(x =>
                  x.Booth.SectorId == s.Id &&
                  x.Booth.Sector.CreatedByUserId == qp.UserId  && x.UserId == qp.UserId &&
                  x.Status),

          PravasiVoter = _context.Tbl_PravasiVoter
              .Count(x =>
                  x.Booth.SectorId == s.Id &&
                  x.Booth.Sector.CreatedByUserId == qp.UserId && x.UserId == qp.UserId &&
                  x.Status)
      },
      ct: ct
  );
            
        }
        public async Task<List<SectorReportExportRow>> GetSectorReportExportAsync(
    SectorReportFilter filter,
    CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(filter.UserId))
                throw new ArgumentException("UserId is required for sector reports.");

            var query = _context.Tbl_Sector
                .AsNoTracking()
                .Where(s => s.CreatedByUserId == filter.UserId);

            if (filter.SectorId.HasValue)
                query = query.Where(s => s.Id == filter.SectorId.Value);

            if (filter.MandalId.HasValue)
                query = query.Where(s => s.MandalId == filter.MandalId.Value);

            if (filter.CastId.HasValue)
                query = query.Where(s => s.CastId == filter.CastId.Value);

            if (filter.VillageId.HasValue)
                query = query.Where(s => s.Villages.Any(v => v.VillageId == filter.VillageId.Value));

            // Projection
            var result = await query
                .Select(s => new SectorReportExportRow
                {
                    SectorId = s.Id,
                    SectorName = s.SectorName ?? string.Empty,
                    SectorSanyojak = s.IsSectorSanyojak ? (s.InchargeName ?? string.Empty) : string.Empty,
                    Mobile = s.IsSectorSanyojak ? (s.PhoneNumber ?? string.Empty) : string.Empty,
                    Cast = s.Cast != null ? s.Cast.CastName : null,

                    Villages = s.Villages != null
                        ? s.Villages.Select(v => new VillageExpDto
                        {
                            Id = v.VillageId,
                            Name = v.Village != null ? v.Village.VillageName : null
                        }).ToList()
                        : new List<VillageExpDto>(),

                    TotalBooth = _context.Tbl_Booth
                        .Count(x => x.SectorId == s.Id && x.Status),

                    TotalVotes = _context.Tbl_BoothVoter
                        .Where(x => x.Booth.SectorId == s.Id && x.Booth.Sector.CreatedByUserId == filter.UserId && x.Status)
                        .Sum(x => (int?)x.TotalVoter) ?? 0,

                    SeniorCitizen = _context.Tbl_SeniorDisabled
                        .Count(x => x.Booth.SectorId == s.Id && x.Booth.Sector.CreatedByUserId == filter.UserId &&
                                    x.UserId == filter.UserId && x.TypeId == 1 && x.Status),

                    Handicap = _context.Tbl_SeniorDisabled
                        .Count(x => x.Booth.SectorId == s.Id && x.Booth.Sector.CreatedByUserId == filter.UserId &&
                                    x.UserId == filter.UserId && x.TypeId == 2 && x.Status),

                    DoubleVoter = _context.Tbl_DoubleVoter
                        .Count(x => x.Booth.SectorId == s.Id && x.Booth.Sector.CreatedByUserId == filter.UserId &&
                                    x.UserId == filter.UserId && x.Status),

                    PravasiVoter = _context.Tbl_PravasiVoter
                        .Count(x => x.Booth.SectorId == s.Id && x.Booth.Sector.CreatedByUserId == filter.UserId &&
                                    x.UserId == filter.UserId && x.Status)
                })
                .ToListAsync(ct);

            return result;
        }
        public async Task<Tbl_Sector?> GetByIdAsync(int id)
            => await _context.Tbl_Sector
                .Include(s => s.Mandal)
                .Include(s => s.Villages)
                .Include(s => s.Category)
                .Include(s => s.Cast)
                .FirstOrDefaultAsync(s => s.Id == id && s.Status);
        public async Task<List<Tbl_Sector>?> GetByMandalIdAsync(int id)
      => await _context.Tbl_Sector
          .Where(s => s.MandalId == id && s.Status)
           .Include(s => s.Mandal)
           .Include(s => s.Villages)
           .Include(s => s.Category)
           .Include(s => s.Cast)
           .ToListAsync();
        
        public async Task<int> AddAsync(Tbl_Sector sector)
        {
            try
            {
                await _context.Tbl_Sector.AddAsync(sector);
                return await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        public async Task<int> UpdateAsync(Tbl_Sector sector)
        {
            _context.Tbl_Sector.Update(sector);
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tbl_Sector sector)
        {
             sector.Delete();
            _context.Tbl_Sector.Update(sector);
            await _context.SaveChangesAsync();
        }

        public  async Task<IReadOnlyList<SectorIncahrgeDto>> GetIncahrgeByIdAsync(string userId)
        {
            return  await _context.Tbl_Sector.Where(m => m.CreatedByUserId == userId).Select(s => new SectorIncahrgeDto
            {
                UserId = s.UserId,
                SectorId = s.Id,
                SectorIncharge = s.InchargeName
            }).ToListAsync();
            
        }


        public async Task<List<VillageDto>> GetAllSectorVillagesByUserId(
      SectorVillageQueryParams qp,
    CancellationToken ct = default)
        {
            return await _context.Tbl_SectorVillage
                .AsNoTracking()
                .Where(v =>
                    v.Status &&
                    v.Sector.CreatedByUserId == qp.UserId)
                .Select(v => new VillageDto
                {
                    Id = v.VillageId,
                    Name = v.Village != null ? v.Village.VillageName : null
                })
                .Distinct()
                .ToListAsync(ct);
        }
    }
}
