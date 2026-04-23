using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
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
            var query = _context.Tbl_PannaPramukh
              .AsNoTracking()
              .Where(b =>
                  (!qp.Id.HasValue || b.Id == qp.Id) &&
                  (!qp.BoothId.HasValue || b.BoothId == qp.BoothId ));


            Expression<Func<Tbl_PannaPramukh, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Booth.BoothNumber.Equals(Convert.ToInt32(term)) ||
                    b.PannaPramukhName.ToLower().Contains(term) ||
                    b.PannaNumber.Equals(Convert.ToInt32(term)) ||
                    b.Address.ToLower().Contains(term) ||
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
                },
                ct: ct
                );
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
