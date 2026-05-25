using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
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
                var totalCount = await _context.Tbl_Pradhan.CountAsync(x => x.Status && x.UserId == qp.UserId);
                var query = _context.Tbl_Pradhan
               .AsNoTracking()
               .AsQueryable();

                var boothIds = qp.GetBoothIds();
                var SectorIds = qp.GetSectorIds();
                var mandalIds = qp.GetMandalIds();

                // ✅ FIX 1: query = assign karo, sirf query.Where nahi
                query = query.Where(b =>
                  b.Status && b.UserId == qp.UserId &&
                    (!qp.Id.HasValue || b.Id == qp.Id));
                    //b.UserId == qp.UserId);                 // ✅ FIX 2: closing brace sahi jagah

                //if (boothIds.Any())
                //    query = query.Where(b => boothIds.Contains(b.));

                //if (SectorIds.Any())
                //    query = query.Where(b => b.sec);

                //if (villageIds.Any())
                //    query = query.Where(b => b.Villages.Any(v => villageIds.Contains(v.VillageId)));

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
                   TotalCount = totalCount,
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

        public async Task<List<PradhanExportRow>> GetPradhanExportAsync(PradhanExportFilter qp)
        {
            var query = _context.Tbl_Pradhan
                .AsNoTracking()
                .AsQueryable();

            query = query.Where(b =>
                b.Status &&
                ( b.UserId == qp.UserId) &&
                (!qp.Id.HasValue || b.Id == qp.Id));

            // Optional filters if implemented
            var boothIds = qp.BoothId.HasValue ? new[] { qp.BoothId.Value } : Array.Empty<int>();
            if (boothIds.Any())
                query = query.Where(b => b.Villages.Any(v => boothIds.Contains(v.VillageId)));

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                query = query.Where(b =>
                    b.Name.ToLower().Contains(term) ||
                    b.Designation.DesignationName.ToLower().Contains(term) ||
                    b.Villages.Any(v => v.Village.VillageName.ToLower().Contains(term))
                );
            }

            return await query.Select(p => new PradhanExportRow
            {
                Id = p.Id,
                Name = p.Name,
                Designation = p.Designation.DesignationName,
                Gender = ((VidhanSabha.Domain.Enums.Gender)p.Gender).ToString(),
                Contact = p.Contact,
                Villages = string.Join(", ", p.Villages.Select(v => v.Village.VillageName))
            }).ToListAsync();
        }

    }
}
