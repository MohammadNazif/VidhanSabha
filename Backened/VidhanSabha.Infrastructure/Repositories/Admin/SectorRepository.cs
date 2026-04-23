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

        public async Task<PagedResult<SectorResponseDto>> GetAllAsync(SectorQueryParams qp,CancellationToken ct)
        {
            var query = _context.Tbl_Sector
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
                   VillageId = s.VillageId,
                   VillageName = s.Village.VillageName,
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

            
        
            

        public async Task<Tbl_Sector?> GetByIdAsync(int id)
            => await _context.Tbl_Sector
                .Include(s => s.Mandal)
                .Include(s => s.Village)
                .Include(s => s.Category)
                .Include(s => s.Cast)
                .FirstOrDefaultAsync(s => s.Id == id && s.Status);
        public async Task<List<Tbl_Sector>?> GetByMandalIdAsync(int id)
      => await _context.Tbl_Sector
          .Where(s => s.MandalId == id && s.Status)
           .Include(s => s.Mandal)
           .Include(s => s.Village)
           .Include(s => s.Category)
           .Include(s => s.Cast)
           .ToListAsync();
        
        public async Task AddAsync(Tbl_Sector sector)
        {
            await _context.Tbl_Sector.AddAsync(sector);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tbl_Sector sector)
        {
            _context.Tbl_Sector.Update(sector);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tbl_Sector sector)
        {
             sector.Delete();
            _context.Tbl_Sector.Update(sector);
            await _context.SaveChangesAsync();
        }
    }
}
