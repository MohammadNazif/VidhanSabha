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

        public Task<List<BoothNumberDto>> BoothNumberExistsAsync()
        {
          var res = _context.Tbl_Booth
             
               .Select(b => new BoothNumberDto
             {
                 BoothId = b.Id,
                 BoothNumber = b.BoothNumber,
                 BoothName = b.PollingStationName
             }).ToListAsync();
            return res;
        }

        public async Task Delete(Tbl_Booth booth)
        {
             _context.Tbl_Booth.Update(booth);
              await _context.SaveChangesAsync();
            
        }

        public async Task<PagedResult<BoothResponseDto>> GetAllAsync(
          BoothQueryParams qp, CancellationToken ct = default)
        {
            var query = _context.Tbl_Booth
                .AsNoTracking()
                .Where(b => b.Status)
                .Where(b =>
                    (!qp.MandalId.HasValue || b.MandalId == qp.MandalId) &&
                    (!qp.SectorId.HasValue || b.SectorId == qp.SectorId));

            Expression<Func<Tbl_Booth, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
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
