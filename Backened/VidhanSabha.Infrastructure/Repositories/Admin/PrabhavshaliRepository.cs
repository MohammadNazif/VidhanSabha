using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class PrabhavshaliRepository:BaseRepository<Tbl_PrabhavshaliVyakti>,IPrabhavshaliRepository
    {
        public PrabhavshaliRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<int> AddAsync(Tbl_PrabhavshaliVyakti prabhav, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_PrabhavshaliVyakti.AddAsync(prabhav);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }


        }
        public int Update(Tbl_PrabhavshaliVyakti prabhav)
        {
            try
            {
                _context.Tbl_PrabhavshaliVyakti.Update(prabhav);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_PrabhavshaliVyakti prabhav)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PrabhavshaliResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default)
        {
            var result = await _context.Tbl_PrabhavshaliVyakti
                .Select(m => new PrabhavshaliResponseDto
                {
                    Id = m.Id,
                    BoothId = m.BoothId,
                    BoothNumber = m.Booth.BoothNumber,
                    DesignationId=m.DesignationId,
                    DesignationName=m.Designation.DesignationName,
                    Name = m.Name,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.Name,
                    CastId = m.CastId,
                    CastName = m.Cast.CastName,
                    Mobile = m.Mobile,
                    Description=m.Description,
                    Villages = m.Villages.Select(v => new VillageResponseDtos
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName
                    }).ToList(),
                }).ToListAsync();
            return result;
        }


        public async Task<Tbl_PrabhavshaliVyakti?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_PrabhavshaliVyakti
                 .Include(p => p.Villages)
                 .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PrabhavshaliResponseDesinIdDto>> GetByDesgIdAsync(int desgid)
        {
            try
            {
                return await _context.Tbl_PrabhavshaliVyakti
                    .Where(m => m.DesignationId == desgid)
                    .Select(m => new PrabhavshaliResponseDesinIdDto
                    {
                        Id = m.Id,
                        DesgId = m.DesignationId,
                        SectorId = m.Booth.Sector.Id,
                        SectorName = m.Booth.Sector.SectorName,
                        SectoeSanyojak = m.Booth.Sector.InchargeName,
                        BoothId = m.BoothId,
                        BoothNumber = m.Booth != null ? m.Booth.BoothNumber : 0,
                        BoothSanyojak = m.Booth.Sanyojak.InchargeName,
                        DesignationName = m.Designation != null ? m.Designation.DesignationName : "Unknown",
                        Name = m.Name,
                        CategoryId = m.CategoryId,
                        CategoryName = m.Category != null ? m.Category.Name : "N/A",
                        CastId = m.CastId,
                        CastName = m.Cast != null ? m.Cast.CastName : "N/A",
                        Mobile = m.Mobile,
                        Description = m.Description,
                        Villages = m.Villages.Select(v => new VillageResponseDtos
                        {
                            VillageId = v.VillageId,
                            VillageName = v.Village != null ? v.Village.VillageName : "N/A"
                        }).ToList(),
                    }).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
