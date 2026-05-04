using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
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

        public async Task<PagedResult<SectorResponseDto>> GetAllAsync(SectorQueryParams qp,int? vidhanId,CancellationToken ct)
        {
            var query = _context.Tbl_Sector.Where(x => x.Mandal.VidhanId == vidhanId)
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) 
                   
                   );

            Expression<Func<Tbl_Sector, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.SectorName.ToLower().Contains(term);
                //b.Village.VillageName.ToLower().Contains(term) ||
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Id,
               projection: s => new SectorResponseDto
               {
                   Id = s.Id,
                   MandalId = s.MandalId,
                   MandalName = s.Mandal.Name,
                   Villages = s.Villages.Select(v => new VillageResponseDto
                   {
                       VillageId = v.VillageId,
                       VillageName = v.Village.VillageName
                   }).ToList(),
                   SectorName = s.SectorName,
                   IsSectorSanyojak = s.IsSectorSanyojak,
                   InchargeName = s.InchargeName,
                   Age = s.Age,
                   FatherName = s.FatherName,
                   CategoryId = s.CategoryId,
                   CategoryName = s.Category.Name,
                   CastId = s.CastId,
                   CastName = s.Cast.CastName,
                   EducationLevel = s.EducationLevel,
                   PhoneNumber = s.PhoneNumber,
                   Address = s.Address,
                   ProfileImage = s.ProfileImage,
                   Status = s.Status
               },
               ct:ct
               );
        }

        public async Task<PagedResult<SectorReportDto>> GetAllSectorReports(SectorQueryParams qp,CancellationToken ct = default)
        {
            var query = _context.Tbl_Sector
                .AsNoTracking()
                .Where(s =>
                    (!qp.MandalId.HasValue || s.MandalId == qp.MandalId) &&
                    (!qp.SectorId.HasValue || s.Id == qp.SectorId) &&
                    (!qp.BoothId.HasValue || s.Booth.Id == qp.BoothId) &&
                    (!qp.CastId.HasValue || s.CastId == qp.CastId) &&
                    (!qp.VillageId.HasValue ||
                        (
                            // Sector Village (single)
                            (s.Villages.Any(v => v.VillageId == qp.VillageId)) ||

                            // Booth Villages
                            (s.Booth != null &&
                             s.Booth.Villages.Any(v => v.VillageId == qp.VillageId))
                        ))
                );

            // 🔍 SEARCH
            Expression<Func<Tbl_Sector, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();

                search = s =>
                    s.SectorName.ToLower().Contains(term) ||
                    s.InchargeName.ToLower().Contains(term) ||
                    s.PhoneNumber.Contains(term) ||

                    // Mandal
                    (s.Mandal != null &&
                     s.Mandal.Name.ToLower().Contains(term)) ||

                    // Booth
                    (s.Booth != null &&
                     s.Booth.PollingStationName.ToLower().Contains(term)) ||

                    // Sanyojak
                    (s.Booth != null &&
                     s.Booth.Sanyojak != null &&
                     (
                        s.Booth.Sanyojak.InchargeName.ToLower().Contains(term) ||
                        s.Booth.Sanyojak.PhoneNumber.Contains(term)
                     )) ||

                    // Villages
                    (s.Booth != null &&
                     s.Booth.Villages.Any(v =>
                        v.Village != null &&
                        v.Village.VillageName.ToLower().Contains(term)));
            }

            // 🚀 PAGINATION + PROJECTION (same pattern)
            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: s => s.Id,
                projection: s => new SectorReportDto
                {
                    // 🔹 Mandal
                    MandalId = s.Mandal.Id,
                    MandalName = s.Mandal.Name,

                    // 🔹 Sector
                    SectorId = s.Id,
                    SectorName = s.SectorName,
                    InchargeName = s.InchargeName,
                    Age = s.Age,
                    FatherName = s.FatherName,
                    CastId = s.CastId,
                    CastName = s.Cast != null ? s.Cast.CastName : null,
                    EducationLevel = s.EducationLevel,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    ProfileImage = s.ProfileImage,

                    // 🔥 Sector Village (single case)
                    SectorVillages = s.Villages != null
                        ? new List<VillageDto>
                        {
                    new VillageDto
                    {
                        Id = s.Villages.Select(v=>v.VillageId).FirstOrDefault(),
                        Name = s.Villages.Select(v=>v.Village.VillageName).FirstOrDefault()
                    }
                        }
                        : new List<VillageDto>(),

                    // 🔹 Booth
                    Booth = s.Booth != null ? new BoothDto
                    {
                        Id = s.Booth.Id,
                        BoothNumber = s.Booth.BoothNumber,
                        PollingStationName = s.Booth.PollingStationName,

                        Sanyojak = s.Booth != null && s.Booth.Sanyojak != null
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
                },
                ct: ct
            );
        }
        public async Task<PagedResult<AdminSectorReportsDto>> GetAllAdminSectorReports(SectorQueryParams qp,CancellationToken ct = default)
        {
            var query = _context.Tbl_Sector
                .AsNoTracking()
                .Where(s =>
                   
                    (!qp.SectorId.HasValue || s.Id == qp.SectorId) &&
                    (!qp.CastId.HasValue || s.CastId == qp.CastId) 
                );

            // 🔍 SEARCH
            Expression<Func<Tbl_Sector, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();

                search = s =>
                    s.SectorName.ToLower().Contains(term) ||
                    s.InchargeName.ToLower().Contains(term) ||
                    s.PhoneNumber.Contains(term)
                    ;
            }

            // 🚀 PAGINATION + PROJECTION (same pattern)
            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: s => s.Id,
                projection: s => new AdminSectorReportsDto
                {
                   SectorId=s.Id,
                   SectorName=s.SectorName,
                   SectorSanyojak=s.InchargeName,
                   Mobile=s.PhoneNumber,
                   Cast=s.Cast.CastName,
                   Villages = s.Villages != null
                        ? new List<VillageDto>
                        {
                    new VillageDto
                    {
                         Id = s.Villages.Select(v=>v.VillageId).FirstOrDefault(),
                        Name = s.Villages.Select(v=>v.Village.VillageName).FirstOrDefault()
                    }
                        }
                        : new List<VillageDto>(),
                   TotalBooth=_context.Tbl_Booth.Count(x=>x.SectorId==s.Id && x.Status),
                    TotaVotes =
                    _context.Tbl_SeniorDisabled.Count(x => x.Booth.SectorId == s.Id && x.TypeId == 1 && x.Status) +
                    _context.Tbl_SeniorDisabled.Count(x => x.Booth.SectorId == s.Id && x.TypeId == 2 && x.Status) +
                    _context.Tbl_DoubleVoter.Count(x => x.Booth.SectorId == s.Id && x.Status) +
                    _context.Tbl_PravasiVoter.Count(x => x.Booth.SectorId == s.Id && x.Status),

                    SeniorCitizen = _context.Tbl_SeniorDisabled
                    .Count(x => x.Booth.SectorId == s.Id && x.TypeId == 1 && x.Status),

                    Handicap = _context.Tbl_SeniorDisabled
                    .Count(x => x.Booth.SectorId == s.Id && x.TypeId == 2 && x.Status),

                    DoubleVoter = _context.Tbl_DoubleVoter
                    .Count(x => x.Booth.SectorId == s.Id && x.Status),

                    PravasiVoter = _context.Tbl_PravasiVoter
                    .Count(x => x.Booth.SectorId == s.Id && x.Status)
                },
                ct: ct
            );
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
    }
}
