using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;
using VidhanSabha.Infrastructure.Extensions;
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

        public async Task<PagedResult<PrabhavshaliResponseDto>> GetAllAsync(PrabhavshaliQueryParams qp, CancellationToken ct = default)
        {
            try
            {

             //   var vidhanSabhaId = await _context.Tbl_StatePrabhari
             //.Where(u => u.userId == qp.UserId)
             //.Select(u => u.VidhansabhaId)
             //.FirstOrDefaultAsync();

                var query = _context.Tbl_PrabhavshaliVyakti
                .AsNoTracking();
                var villageIds = qp.GetVillageIds();
                var boothIds = qp.GetBoothIds();
                var designationIds = qp.GetDesignationIds();

                if (qp.rolefilterflag && (qp.Role == PrabhariRole.BoothSanyojak.ToString() || qp.Role == PrabhariRole.SectorSanyojak.ToString()))
                {
                    query = query.Where(f => f.Role == qp.Role.ToString());
                }
                if (villageIds.Any())
                {
                    query = query.Where(b => b.Villages.Any(v => villageIds.Contains(v.VillageId)));
                }
                if(designationIds.Any())
                {
                    query = query.Where(b => designationIds.Contains(b.DesignationId));
                }
                if(boothIds.Any())
                {
                    query = query.Where(b => boothIds.Contains(b.BoothId));
                }
                query = query.Where(b =>
                    (b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId || b.CreatedsectorUserId == qp.UserId) &&
                    (!qp.Id.HasValue || b.Id == qp.Id) &&
                    (!qp.BoothId.HasValue || b.BoothId == qp.BoothId) &&
                    (!qp.CastId.HasValue || b.CastId == qp.CastId) && (b.Booth.Mandal.Status && b.Booth.Sector.Status)
                );

                Expression<Func<Tbl_PrabhavshaliVyakti, bool>>? search = null;

                if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
                {
                    var term = qp.SearchTerm.Trim().ToLower();
                    search = b =>
                        b.Booth.BoothNumber.ToString().Contains(term) ||
                        b.Name.ToLower().Contains(term) ||
                        b.Mobile.ToLower().Contains(term) ||
                        b.Cast.CastName.ToLower().Contains(term) ||
                        b.Category.Name.ToLower().Contains(term) ||
                        b.Mobile.ToLower().Contains(term) ||
                        b.Designation.DesignationName.ToLower().Contains(term);
                }

                return await query.ToPagedResultAsync(
                   queryParams: qp,
                   searchPredicate: search,
                   defaultSort: b => b.Booth.BoothNumber,
                   projection: m => new PrabhavshaliResponseDto
                   {
                       Id = m.Id,
                       BoothId = m.BoothId,
                       BoothNumber = m.Booth.BoothNumber,
                       DesignationId = m.DesignationId,
                       DesignationName = m.Designation.DesignationName,
                       Name = m.Name,
                       CategoryId = m.CategoryId,
                       CategoryName = m.Category.Name,
                       CastId = m.CastId,
                       CastName = m.Cast.CastName,
                       Mobile = m.Mobile,
                       Description = m.Description,
                       Villages = m.Villages.Select(v => new VillageResponseDtos
                       {
                           VillageId = v.VillageId,
                           VillageName = v.Village.VillageName
                       }).ToList(),
                   }, ct: ct);
            }
            catch (Exception)
            {
                throw;
            }
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
        public async Task<List<prabhavsaliExportRow>> GetExportByDesgIdAsync(PrabhavshaliQueryParams qp)
        {
            return await _context.Tbl_PrabhavshaliVyakti
                .Where(m =>
    (qp.designationId == null || m.DesignationId == qp.designationId) &&
    (m.UserId == qp.UserId || m.CreatedToUserId == qp.UserId || m.CreatedsectorUserId == qp.UserId) 
   )
                .Select(m => new prabhavsaliExportRow
                {
                    BoothNumber = m.Booth != null ? m.Booth.BoothNumber : 0,
                    Village = string.Join(", ", m.Villages
                        .Select(v => v.Village != null ? v.Village.VillageName : "N/A")),

                    Name = m.Name,
                    Designation = m.Designation != null ? m.Designation.DesignationName : "Unknown",
                    MobileNumber = m.Mobile,
                    Category = m.Category != null ? m.Category.Name : "N/A",
                    Cast = m.Cast != null ? m.Cast.CastName : "N/A",
                    Description = m.Description
                })
                .ToListAsync();
        }
        public async Task<List<PrabhavshaliResponseDesinIdDto>> GetByDesgIdAsync(int desgid,string userId)
        {
            try
            {
                return await _context.Tbl_PrabhavshaliVyakti
                    .Where(m => m.DesignationId == desgid && (m.UserId == userId || m.CreatedToUserId == userId))
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
