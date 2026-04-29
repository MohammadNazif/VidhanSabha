using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs;
using VidhanSabha.Application.Pannels.Admin.Pradhan.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    internal class PradhanRepository : BaseRepository<Tbl_Pradhan>, IPradhanRepository
    {
        private readonly DatabaseContext _context;
        public PradhanRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public Task<int> AddAsync(Tbl_Pradhan pradhan, CancellationToken ct = default)
        {
            try
            {
                _context.Tbl_Pradhan.AddAsync(pradhan);
                return _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public void Delete(Tbl_Pradhan pradhan)
        {
            try
            {
                _context.Tbl_Pradhan.Remove(pradhan);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public int Update(Tbl_Pradhan pradhan)
        {
            try
            {
                _context.Tbl_Pradhan.Update(pradhan);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }
        public async Task<Tbl_Pradhan?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_Pradhan
                 .Include(p => p.Villages)
                 .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task<PagedResult<PradhanResponseDto>> GetAllAsync(PradhanQueryParams qp, CancellationToken ct = default)
        {
            try
            {
                var query = _context.Tbl_Pradhan
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) && (b.UserId == qp.UserId)
                   //(!qp.SectorId.HasValue || b.Booth.Sector.Id == qp.SectorId) &&
                   //(!qp.BoothId.HasValue || b.Booth.Id == qp.BoothId)
                   );

                Expression<Func<Tbl_Pradhan, bool>>? search = null;

                if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
                {
                    var term = qp.SearchTerm.Trim().ToLower();
                    search = b =>
                        //b.Booth.BoothNumber.Equals(Convert.ToInt32(term)) ||
                        b.Name.ToLower().Contains(term) ||
                        b.Villages.Select(v => v.Village.VillageName).FirstOrDefault().ToLower().Contains(term) ||
                        b.Designation.DesignationName.ToLower().Contains(term);
                }

                return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Id,
               projection: p => new PradhanResponseDto
               {
                   Id = p.Id,
                   Name = p.Name,
                   DesignationId = p.DesignationId,
                   DesignationName = p.Designation.DesignationName,
                   Contact = p.Contact,

                   Gender = p.Gender,
                   GenderValue = ((VidhanSabha.Domain.Enums.Gender)p.Gender).ToString(),

                   Villages = p.Villages.Select(v => new VillageResponseDtos
                   {
                       VillageId = v.VillageId,
                       VillageName = v.Village.VillageName
                   }).ToList(),
               },ct:ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

    }
}
