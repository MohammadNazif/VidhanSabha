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
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
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
            catch(Exception ex)
            {
                throw ex;
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
                .Where(b => mandalIds.Contains(b.MandalId) && b.Status)
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
        .Where(b => b.Mandal.VidhanId == vidhanId);

            var mandalIds = qp.GetMandalIds();
            var sectorIds = qp.GetSectorIds();

            if (mandalIds.Any())
            {
                query = query.Where(b => mandalIds.Contains(b.MandalId));
            }

            if (sectorIds.Any())
                query = query.Where(b => sectorIds.Contains(b.SectorId));

            if (!string.IsNullOrEmpty(qp.UserId))
                query = query.Where(b => b.UserId == qp.UserId);
            Expression<Func<Tbl_Booth, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                   b.BoothNumber.ToString().Contains(term) || 
                    b.PollingStationName.ToLower().Contains(term) ||
                    b.PollingStationLocation.ToLower().Contains(term) ||
                    b.Mandal.Name.ToLower().Contains(term) ||
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
                query = query.Where(b => b.UserId == qp.UserId);

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
          BoothQueryParams qp,int? vidhanId, CancellationToken ct = default)
        {
            var query = _context.Tbl_Booth
                .AsNoTracking()
                //.Where(b => b.Mandal.VidhanId == vidhanId)
                .Where(b =>
                   (b.UserId == qp.UserId));
                    //(!qp.MandalId.HasValue || b.MandalId == qp.MandalId) && (b.UserId == qp.UserId) &&
                    //(!qp.SectorId.HasValue || b.SectorId == qp.SectorId));

            Expression<Func<Tbl_Booth, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {;
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.PollingStationName.ToLower().Contains(term) ||
                    b.PollingStationLocation.ToLower().Contains(term) ||
                    b.Mandal.Name.ToLower().Contains(term) ||
                    b.Sector.SectorName.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: b => b.BoothNumber,
                projection: b => new BoothReportsDto
                {

                    BoothId = b.Id,
                    BoothNo = b.BoothNumber,
                    PollingStation = b.PollingStationName,
                    BoothAdhyaksh=b.Sanyojak.InchargeName,
                    Mobile=b.Sanyojak.PhoneNumber,
                    Villages = b.Villages.Select(v => new VillageResponseDto
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName,
                        HasAnshik = v.HasAnshik
                    }).ToList(),
                    Cast=b.Sanyojak.Cast.CastName,

                    TotalVotes =
                    _context.Tbl_SeniorDisabled.Count(x => x.BoothId == b.Id && x.TypeId == 1 && x.Status) +
                    _context.Tbl_SeniorDisabled.Count(x => x.BoothId == b.Id && x.TypeId == 2 && x.Status) +
                    _context.Tbl_DoubleVoter.Count(x => x.BoothId == b.Id && x.Status) +
                    _context.Tbl_PravasiVoter.Count(x => x.BoothId == b.Id && x.Status),

                    SeniorCitizen = _context.Tbl_SeniorDisabled
                    .Count(x => x.BoothId == b.Id && x.TypeId==1 && x.Status),

                    Handicap = _context.Tbl_SeniorDisabled
                    .Count(x => x.BoothId == b.Id && x.TypeId==2 && x.Status),

                    DoubleVotes = _context.Tbl_DoubleVoter
                    .Count(x => x.BoothId == b.Id && x.Status),

                    Pravasi = _context.Tbl_PravasiVoter
                    .Count(x => x.BoothId == b.Id && x.Status)
                },
                ct: ct
            );
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

        public async Task<List<BoothInchargeResponse>> GetInchargeByBoothIdAsync(int? boothId, CancellationToken ct)
        {
            var query = _context.Tbl_BoothSanyojak.AsQueryable();

            if (boothId.HasValue)
            {
                query = query.Where(x => x.BoothId == boothId && x.Status);
            }

            return await query
                .Where( x =>x.Status)
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
    }
}
