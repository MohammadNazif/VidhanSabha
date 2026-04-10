using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class BoothRepository : BaseRepository<Tbl_Booth>, IBoothRepository
    {
        public BoothRepository(DatabaseContext context ) : base(context)
        { 
        }
        public async Task AddAsync(Tbl_Booth booth, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_Booth.AddAsync(booth, ct);
                await _context.SaveChangesAsync();
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

        public async Task<List<BoothResponseDto>> GetAllAsync(
    int? mandalId = null,
    int? sectorId = null,
    CancellationToken ct = default)
        {
            var query = _context.Tbl_Booth
                .Where(b => (!mandalId.HasValue || b.MandalId == mandalId) &&
                            (!sectorId.HasValue || b.SectorId == sectorId))
                .Select(b => new BoothResponseDto
                {
                    Id = b.Id,
                    MandalId = b.MandalId,
                    MandalName = b.Mandal.Name,        // ✅ JOIN — already working
                    SectorId = b.SectorId,
                    SectorName = b.Sector.SectorName,  // ✅ JOIN — already working
                    BoothNumber = b.BoothNumber,
                    PollingStationName = b.PollingStationName,
                    PollingStationLocation = b.PollingStationLocation,
                    IsBoothSanyojak = b.IsBoothSanyojak,

                    // ✅ Single JOIN to Tbl_Village — no subquery
                    Villages = b.Villages.Select(v => new VillageResponseDto
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName,  // ✅ JOIN
                        HasAnshik = v.HasAnshik
                    }).ToList(),

                    // ✅ Single JOIN to Tbl_Cast — no subquery
                    Sanyojak = b.Sanyojak != null ? new SanyojakResponseDto
                    {
                        InchargeName = b.Sanyojak.InchargeName,
                        Age = b.Sanyojak.Age,
                        FatherName = b.Sanyojak.FatherName,
                        CategoryId = b.Sanyojak.CategoryId,
                        CastId = b.Sanyojak.CastId,
                        CastName = b.Sanyojak.Cast.CastName,  // ✅ JOIN
                        EducationLevel = b.Sanyojak.EducationLevel,
                        PhoneNumber = b.Sanyojak.PhoneNumber,
                        Address = b.Sanyojak.Address
                    } : null
                });

            return await query.AsNoTracking().ToListAsync(ct);
        }
        public async Task<Tbl_Booth?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Tbl_Booth
                .Include(b => b.Villages)    
                .Include(b => b.Sanyojak)   
                .FirstOrDefaultAsync(b => b.Id == id, ct);
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
