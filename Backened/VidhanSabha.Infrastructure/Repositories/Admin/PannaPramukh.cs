using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class PannaPramukh : BaseRepository<Tbl_PannaPramukh>, IPannaPramukhRepository
    {
        public PannaPramukh(DatabaseContext context) : base(context)
        {
         
        }
        public async Task AddAsync(Tbl_PannaPramukh panna, CancellationToken ct = default)
        {
            await _context.Tbl_PannaPramukh.AddAsync(panna);
             _context.SaveChanges();
            
        }

        public void Delete(Tbl_PannaPramukh panna)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<PannaPramukhResponseDto>> GetAllAsync(PannaPramukhQueryParams qp, CancellationToken ct = default)
        {

            //var vidhanSabhaId = await _context.Tbl_StatePrabhari
            // .Where(u => u.userId == qp.UserId)
            // .Select(u => u.VidhansabhaId)
            // .FirstOrDefaultAsync();

            var query = _context.Tbl_PannaPramukh
              .AsNoTracking();

              var villageIds = qp.GetVillageIds();
              var boothIds = qp.GetBoothIds();

            if (qp.rolefilterflag && (qp.Role == PrabhariRole.BoothSanyojak.ToString() || qp.Role == PrabhariRole.SectorSanyojak.ToString()))
            {
                query = query.Where(f => f.Role == qp.Role.ToString());
            }
            if (villageIds?.Count > 0)
                     query = query.Where(b =>
                    b.Villages.Any(v => villageIds.Contains(v.VillageId)));

                   if (boothIds?.Count > 0)
                     query = query.Where(b =>
                    boothIds.Contains(b.BoothId));

            query = query.Where(b =>
                  (!qp.Id.HasValue || b.Id == qp.Id) &&
                  ( b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId || b.CreatedsectorUserId == qp.UserId) &&
                  ( b.Booth.Sector.Status ) && (b.Booth.Mandal.Status) &&
                  ((!qp.BoothId.HasValue || b.BoothId == qp.BoothId) && b.Booth.Status)
                  && (b.Booth.Mandal.Status  && b.Booth.Sector.Status ));



            Expression<Func<Tbl_PannaPramukh, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Booth.BoothNumber.ToString().Contains(term) ||
                    b.PannaPramukhName.ToLower().Contains(term) ||
                    b.Address.ToLower().Contains(term) ||
                    b.Cast.CastName.ToLower().Contains(term) ||
                    //b.Village.VillageName.ToLower().Contains(term) ||
                    b.VoterId.ToLower().Contains(term);

            }

            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: b => b.Booth.BoothNumber,
                projection: m => new PannaPramukhResponseDto
                {
                    Id = m.Id,
                    PannaPramukhName = m.PannaPramukhName,
                    PannaNumber = m.PannaNumber,
                    BoothId = m.BoothId,
                    BoothNumber = m.Booth.BoothNumber,
                    CastId = m.CastId,
                    CastName = m.Cast.CastName,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.Name,
                    Address = m.Address,
                    Villages = m.Villages.Select(v => new VillageDto
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName
                    }).ToList(),
                    VoterId = m.VoterId,
                    PhoneNumber = m.PhoneNumber,
                    ProfilePictureUrl  = m.ProfilePicturePath
                },
                ct: ct
                );
        }
        public async Task<List<PannaPramukhExportRow>> GetPannaPramukhExportAsync(PannaPramukhQueryParams qp)
        {
            return await _context.Tbl_PannaPramukh 
                .AsNoTracking()
                .Where(m => m.Status == true && 
                      (m.UserId == qp.UserId || m.CreatedToUserId == qp.UserId || m.CreatedsectorUserId == qp.UserId) &&
                           (!qp.BoothId.HasValue || m.BoothId == qp.BoothId))
                .Select(m => new PannaPramukhExportRow
                {
                    BoothNumber = m.Booth != null ? m.Booth.BoothNumber : 0,

                    Village = m.Booth != null
                        ? string.Join(", ", m.Booth.Villages
                            .Select(v => v.Village != null ? v.Village.VillageName : "N/A"))
                        : "N/A",

                    PannaPramukh = m.PannaPramukhName,
                    PannaNo = m.PannaNumber.ToString(),

                    Cast = m.Cast != null ? m.Cast.CastName : "N/A",
                    VoterId = m.VoterId,
                    Address = m.Address,
                    MobileNumber = m.PhoneNumber
                })
                .ToListAsync();
        }
        public async Task<Tbl_PannaPramukh?> GetByIdAsync(int id)
        {
            try {
                return await _context.Tbl_PannaPramukh
                 .Include(p => p.Villages)
                 .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
          public Task<bool> PannaNumberExistsAsync(int boothId, int pannaNumber, int? excludeId = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public int Update(Tbl_PannaPramukh panna)
            {
            _context.Tbl_PannaPramukh.Update(panna);
           return  _context.SaveChanges();
        }
    }
}
