using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Booth.Dtos;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class BoothRepository : BaseRepository<Tbl_Booth>, IBoothRepository
    {
        public BoothRepository(DatabaseContext context ) : base(context)
        { 
        }
        public async Task<int> AddAsync(Tbl_Booth booth, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_Booth.AddAsync(booth, ct);
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
            {
                // 2601 = Violation of unique index
                throw new NotFoundException("The username or mobile number already exists for an active account.");
            }
        }

        public async Task<List<BoothNumberDto>> BoothNumberExistsAsync(string userId)
        {
            
            var vidhanSabhaId = await _context.Tbl_StatePrabhari
                .Where(u => u.userId == userId)
                .Select(u => u.VidhansabhaId)
                .FirstOrDefaultAsync();

            var mandalIds = await _context.Tbl_Mandal
                .Where(m => m.VidhanId == vidhanSabhaId && m.Status)
                .Select(m => m.Id)
                .ToListAsync();

            var res = await _context.Tbl_Booth
                .Where(b => mandalIds.Contains(b.MandalId) && b.Status || b.CreatedToSectorUserId == userId || b.Sanyojak.UserId == userId)
                .Select(b => new BoothNumberDto
                {
                    BoothId = b.Id,
                    BoothNumber = b.BoothNumber,
                    BoothName = b.PollingStationName
                })
                .ToListAsync();

   
            return res;
        }

        public async Task<List<BoothNumberDto>> BoothBysectorId(string userId)
        {

            var sectorId =  await _context.Tbl_Sector.Where(x => x.UserId == userId).Select(b => b.Id).FirstOrDefaultAsync();
           

            var res = await _context.Tbl_Booth
                .Where(b => b.SectorId == sectorId && b.Status)
                .Select(b => new BoothNumberDto
                {
                    BoothId = b.Id,
                    BoothNumber = b.BoothNumber,
                    BoothName = b.PollingStationName
                })
                .ToListAsync();


            return res;
        }


        public async Task Delete(Tbl_Booth booth)
        {
             _context.Tbl_Booth.Update(booth);
              await _context.SaveChangesAsync();
            
        }

        public async Task<PagedResult<BoothResponseDto>>   GetAllAsync(
          BoothQueryParams qp,int? vidhanId, CancellationToken ct = default)
        {
            var query = _context.Tbl_Booth
           .AsNoTracking()
         .Where(b => !vidhanId.HasValue || b.Mandal.VidhanId == vidhanId.Value);

            var mandalIds = qp.GetMandalIds();
            var sectorIds = qp.GetSectorIds();

            var boothIds = qp.GetBoothIds();

            if (mandalIds.Any())
            {
                query = query.Where(b => mandalIds.Contains(b.MandalId));
            }

            if (sectorIds.Any())
                query = query.Where(b => sectorIds.Contains(b.SectorId));
            if (boothIds.Any())
                query = query.Where(b => boothIds.Contains(b.Id));

            if (!string.IsNullOrEmpty(qp.UserId))
                query = query.Where(b => b.UserId == qp.UserId || b.Sector.UserId == qp.UserId);

            if (qp.rolefilterflag && (qp.Role == PrabhariRole.BoothSanyojak.ToString() || qp.Role == PrabhariRole.SectorSanyojak.ToString()))
            {
                query = query.Where(f => f.Role == qp.Role.ToString());
            }
            Expression<Func<Tbl_Booth, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.BoothNumber.ToString().Contains(term) || 
                    b.PollingStationName.ToLower().Contains(term) ||
                    b.PollingStationLocation.ToLower().Contains(term) ||
                    b.Mandal.Name.ToLower().Contains(term) ||
                    b.Sanyojak.InchargeName.ToLower().Contains(term) || 
                    b.Sanyojak.Cast.CastName.ToLower().Contains(term) ||
                    b.Sector.SectorName.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: b => b.BoothNumber,
                projection: b => new BoothResponseDto
                {
                   
                    Id = b.Id,
                    MandalId = b.MandalId,
                    MandalName = b.Mandal.Name,
                    SectorId = b.SectorId,
                    SectorName = b.Sector.SectorName,
                    UserId = b.Sanyojak.UserId,
                    BoothNumber = b.BoothNumber,
                    PollingStationName = b.PollingStationName,
                    PollingStationLocation = b.PollingStationLocation,
                    Password = b.Login.Password,
                    IsBoothSanyojak = b.IsBoothSanyojak,
                    Villages = b.Villages.Select(v => new VillageResponseDto
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName,
                        HasAnshik = v.HasAnshik
                    }).ToList(),
                    Sanyojak = b.Sanyojak != null ? new SanyojakResponseDto
                    {
                        InchargeName = b.Sanyojak.InchargeName,
                        Age = b.Sanyojak.Age,
                        FatherName = b.Sanyojak.FatherName,
                        CategoryId = b.Sanyojak.CategoryId,
                        CastId = b.Sanyojak.CastId,
                        CastName = b.Sanyojak.Cast.CastName,
                        EducationLevel = b.Sanyojak.EducationLevel,
                        PhoneNumber = b.Sanyojak.PhoneNumber,
                        Profile = b.Sanyojak.ProfileImagePath,
                        Address = b.Sanyojak.Address
                    } : null
                },
                ct: ct
            );
        }

        public async Task<List<BoothExportRow>> GetAllForExportAsync(
     BoothQueryParams qp,
     CancellationToken ct = default)
        {
            var query = _context.Tbl_Booth
                .AsNoTracking()
                .Include(b => b.Mandal)
                .Include(b => b.Sector)
                .Include(b => b.Villages)
                    .ThenInclude(v => v.Village)
                .Include(b => b.Sanyojak)
                    .ThenInclude(s => s.Cast)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(qp.UserId))
                query = query.Where(b => b.UserId == qp.UserId );

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim();

                query = query.Where(b =>
                    EF.Functions.Like(b.PollingStationName, $"%{term}%") ||
                    EF.Functions.Like(b.PollingStationLocation, $"%{term}%") ||
                    EF.Functions.Like(b.Mandal.Name, $"%{term}%") ||
                    EF.Functions.Like(b.Sector.SectorName, $"%{term}%") ||
                    EF.Functions.Like(b.BoothNumber.ToString(), $"%{term}%")
                );
            }

            var data = await query
                .OrderBy(b => b.BoothNumber)
                .Select(b => new
                {
                    b.BoothNumber,
                    MandalName = b.Mandal.Name,
                    SectorName = b.Sector.SectorName,
                    b.PollingStationName,
                    Villages = b.Villages.Select(v => v.Village.VillageName).ToList(),
                    Sanyojak = b.Sanyojak
                })
                .ToListAsync(ct);

            return data.Select(b => new BoothExportRow
            {
                MandalName = b.MandalName,
                SectorName = b.SectorName,
                BoothNumber = b.BoothNumber,
                Village = string.Join(", ", b.Villages),
                PollingStationName = b.PollingStationName,
                BoothAathyaksh = b.Sanyojak?.InchargeName ?? "N/A",
                ContactNumber = b.Sanyojak?.PhoneNumber ?? "N/A",
                CastName = b.Sanyojak?.Cast?.CastName ?? "N/A"
            }).ToList();
        }


        public async Task<PagedResult<BoothReportsDto>> GetAllBoothReports(
          BoothQueryParams qp,
          string userId,
          CancellationToken ct = default)
        {
            var term = qp.SearchTerm?.Trim().ToLower();

            // =========================
            // 🔹 BASE QUERY
            // =========================
            var query = _context.Tbl_Booth
                .AsNoTracking()
                .Where(b => b.UserId == userId);

            var mandalIds = qp.GetMandalIds();
            var villageIds = qp.GetvillageIds();
            var boothIds = qp.GetBoothIds();
            var castIds = qp.GetCastIds();

            if (mandalIds.Any())
                query = query.Where(b => mandalIds.Contains(b.MandalId));

            if (villageIds.Any())
                query = query.Where(b => b.Villages.Any(v => villageIds.Contains(v.VillageId)));

            if (boothIds.Any())
                query = query.Where(b => boothIds.Contains(b.Id));

            if (castIds.Any())
                query = query.Where(b => b.Sanyojak != null && castIds.Contains(b.Sanyojak.CastId));

            // =========================
            // 🔍 SEARCH — baked into query directly
            // =========================
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(b =>
                    b.BoothNumber.ToString().Contains(term) ||
                    (b.PollingStationName != null && b.PollingStationName.ToLower().Contains(term)) ||
                    (b.PollingStationLocation != null && b.PollingStationLocation.ToLower().Contains(term)) ||
                    (b.Mandal != null && b.Mandal.Name != null && b.Mandal.Name.ToLower().Contains(term)) ||
                    (b.Sector != null && b.Sector.SectorName != null && b.Sector.SectorName.ToLower().Contains(term)) ||
                    (b.Sanyojak != null && b.Sanyojak.InchargeName != null && b.Sanyojak.InchargeName.ToLower().Contains(term)) ||
                    (b.Sanyojak != null && b.Sanyojak.PhoneNumber != null && b.Sanyojak.PhoneNumber.Contains(term)));
            }

            // =========================
            // 📦 PAGINATION + PROJECTION
            // =========================
            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: null,  // ✅ already applied above
                defaultSort: b => b.BoothNumber,
                projection: b => new BoothReportsDto
                {
                    BoothId = b.Id,
                    BoothNo = b.BoothNumber,
                    PollingStation = b.PollingStationName,

                    BoothAdhyaksh = b.Sanyojak != null
                        ? b.Sanyojak.InchargeName
                        : null,

                    Mobile = b.Sanyojak != null
                        ? b.Sanyojak.PhoneNumber
                        : null,

                    Cast = b.Sanyojak != null && b.Sanyojak.Cast != null
                        ? b.Sanyojak.Cast.CastName
                        : null,

                    Villages = b.Villages != null
                        ? b.Villages.Select(v => new VillageResponseDto
                        {
                            VillageId = v.VillageId,
                            VillageName = v.Village != null ? v.Village.VillageName : null,
                            HasAnshik = v.HasAnshik
                        }).ToList()
                        : new List<VillageResponseDto>(),

                    TotalVotes = _context.Tbl_BoothVoter
                        .Where(x =>
                            x.BoothId == b.Id &&
                            x.UserId == userId &&
                            x.Status)
                        .Select(x => x.TotalVoter)
                        .FirstOrDefault(),

                    SeniorCitizen = _context.Tbl_SeniorDisabled
                        .Count(x =>
                            x.BoothId == b.Id &&
                            x.TypeId == 1 &&
                            x.Status &&
                            x.UserId == userId),

                    Handicap = _context.Tbl_SeniorDisabled
                        .Count(x =>
                            x.BoothId == b.Id &&
                            x.TypeId == 2 &&
                            x.Status &&
                            x.UserId == userId),

                    DoubleVotes = _context.Tbl_DoubleVoter
                        .Count(x =>
                            x.BoothId == b.Id &&
                            x.Status &&
                            x.UserId == userId),

                    Pravasi = _context.Tbl_PravasiVoter
                        .Count(x =>
                            x.BoothId == b.Id &&
                            x.Status &&
                            x.UserId == userId)
                },
                ct: ct
            );
        }

        public async Task<List<BoothReportExportRow>> GetBoothReportExportAsync(
      BoothReportFilter filter,
      CancellationToken ct = default)
        {
            // =========================
            // 🔹 BASE QUERY
            // =========================
            var query = _context.Tbl_Booth
                .AsNoTracking()
                .Where(b => b.UserId == filter.UserId);

            if (filter.MandalId.HasValue)
                query = query.Where(b => b.MandalId == filter.MandalId.Value);

            if (filter.SectorId.HasValue)
                query = query.Where(b => b.SectorId == filter.SectorId.Value);

            if (filter.BoothId.HasValue)
                query = query.Where(b => b.Id == filter.BoothId.Value);

            // =========================
            // 📦 PROJECTION FOR EXPORT
            // =========================
            var result = await query
                .Select(b => new BoothReportExportRow
                {
                    BoothNumber = b.BoothNumber,
                    PollingStation = b.PollingStationName,
                    //MandalName = b.Mandal != null ? b.Mandal.Name : "",
                    //SectorName = b.Sector != null ? b.Sector.SectorName : "",

                    // Booth In-charge
                    BoothAdhyaksh = b.Sanyojak != null ? b.Sanyojak.InchargeName : null,
                    Mobile = b.Sanyojak != null ? b.Sanyojak.PhoneNumber : null,
                    Cast = b.Sanyojak != null && b.Sanyojak.Cast != null ? b.Sanyojak.Cast.CastName : null,

                    // Villages
                    Villages = b.Villages != null
                        ? b.Villages.Select(v => new VillageExpResponseDto
                        {
                            VillageId = v.VillageId,
                            VillageName = v.Village != null ? v.Village.VillageName : null,
                            HasAnshik = v.HasAnshik
                        }).ToList()
                        : new List<VillageExpResponseDto>(),

                    // Counts
                    TotalVotes = _context.Tbl_BoothVoter
                        .Where(x => x.BoothId == b.Id && x.UserId == filter.UserId && x.Status)
                        .Select(x => x.TotalVoter)
                        .FirstOrDefault(),

                    SeniorCitizen = _context.Tbl_SeniorDisabled
                        .Count(x => x.BoothId == b.Id && x.TypeId == 1 && x.Status && x.UserId == filter.UserId),

                    Handicap = _context.Tbl_SeniorDisabled
                        .Count(x => x.BoothId == b.Id && x.TypeId == 2 && x.Status && x.UserId == filter.UserId),

                    DoubleVotes = _context.Tbl_DoubleVoter
                        .Count(x => x.BoothId == b.Id && x.Status && x.UserId == filter.UserId),

                    Pravasi = _context.Tbl_PravasiVoter
                        .Count(x => x.BoothId == b.Id && x.Status && x.UserId == filter.UserId)
                })
                .ToListAsync(ct);

            return result;
        }

        public Task<Tbl_BoothSanyojak?> GetByBoothIdAsync(int boothId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<Tbl_Booth?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Tbl_Booth
                .Include(b => b.Villages)    
                .Include(b => b.Sanyojak)   
                .FirstOrDefaultAsync(b => b.Id == id, ct);
        }

        public async Task<List<BoothInchargeResponse>> GetInchargeByBoothIdAsync(int? boothId,string userId, CancellationToken ct)
        {
            var query = _context.Tbl_BoothSanyojak.AsQueryable();

            if (boothId.HasValue)
            {
                query = query.Where(x => x.BoothId == boothId && x.Status);
            }

            return await query
                .Where( x =>x.Status && x.Booth.UserId ==userId )
                .Select(x => new BoothInchargeResponse
                {
                    UserId = x.UserId,
                    BoothId = x.BoothId,
                    BoothInchargeName = x.InchargeName
                })
                .ToListAsync(ct);
        }

        public Task SaveAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Tbl_Booth booth, CancellationToken ct)
        {
            _context.Tbl_Booth.Update(booth);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<string> GetUseridbyBoothId(int boothId)
        {
            return await _context.Tbl_BoothSanyojak
                .Where(m => m.BoothId == boothId)
                .Select(b => b.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetadminUseridbyUserId(int boothId)
        {
            return await _context.Tbl_Booth
               .Where(m => m.Id == boothId)
               .Select(b => b.UserId)
               .FirstOrDefaultAsync();
        }

        public async Task<string> GetadminUseridbySectorUserId(int sectorId)
        {
            return await _context.Tbl_Sector
               .Where(m => m.Id == sectorId)
               .Select(b => b.CreatedByUserId)
               .FirstOrDefaultAsync();
        }

        public async Task<string> GetSectorUseridbyBoothId(int boothId)
        {
            var sectorId = await _context.Tbl_Booth.Where(x => x.Id == boothId).Select(b => b.SectorId).FirstOrDefaultAsync();

            return await _context.Tbl_Sector.Where(x => x.Id == sectorId).Select(b => b.UserId).FirstOrDefaultAsync();
        }

        public async Task<string> GetSectorUseridbySectorId(int sectorId)
        {
           
            return await _context.Tbl_Sector.Where(x => x.Id == sectorId).Select(b => b.UserId).FirstOrDefaultAsync();
        }
    }
}
