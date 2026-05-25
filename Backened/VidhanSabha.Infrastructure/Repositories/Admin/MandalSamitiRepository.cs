using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Dtos;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    internal class MandalSamitiRepository : BaseRepository<Tbl_MandalSamiti>, IMandalSamiti
    {
        public MandalSamitiRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<PagedResult<MandalSamitiResponseDto>> GetAllMandalSamitiAsync(
     MandalSamitiQueryParams qp, CancellationToken ct = default)
        {
            try
            {
                var query = _context.Tbl_MandalSamiti
                    .AsNoTracking()
                    // ✅ Global query filter already handles Status — only filter by UserId
                    .Where(x => x.UserId == qp.UserId)
                    // ✅ Explicit includes = single JOIN query, no lazy-load surprises
                    .Include(x => x.Mandal)
                    .Include(x => x.MandalSanyojak);
                Expression<Func<Tbl_MandalSamiti, bool>>? search = null;
                if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
                {
                    var term = qp.SearchTerm.Trim().ToLower();
                    search = b =>
                        b.MandalSanyojak.InchargeName.ToString().Contains(term) ||
                        b.Mandal.Name.ToString().Contains(term) ||
                        b.MandalSanyojak.Contact.ToString().Contains(term);

                }
                return await query.ToPagedResultAsync(
                    queryParams: qp,
                    searchPredicate: null,
                    defaultSort: b => b.Mandal.Name,
                    projection: m => new MandalSamitiResponseDto
                    {
                        Id = m.Id,
                        MandalId = m.MandalId,
                        MandalName = m.Mandal != null ? m.Mandal.Name : null,

                        // ✅ Null-safe: MandalSanyojak is optional
                        MandalAdhayaksh = m.MandalSanyojak != null
                                            ? m.MandalSanyojak.InchargeName
                                            : null,

                        TotalMember = m.TotalMembers,
                         Contact      = m.MandalSanyojak != null ? m.MandalSanyojak.Contact : null
                    },
                    ct: ct
                );
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Tbl_MandalSamiti> GetMandalSamitiByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Tbl_MandalSamiti
                .FirstOrDefaultAsync(x => x.Id == id || x.MandalId == id, ct);
        }


        public async Task<int> InsertMandalSamiti(Tbl_MandalSamiti request)
        {
            try
            {
              
                _context.Tbl_MandalSamiti.Add(request);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateMandalSamiti(Tbl_MandalSamiti request)
        {
            _context.Tbl_MandalSamiti.Update(request); 
        }

        public void InsertMandalSamitiMember(Tbl_MandalSamitiMem request)
        {
            _context.Tbl_MandalSamitiMem.Add(request); 
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await _context.SaveChangesAsync(ct); 
        }

        public async Task<List<MandalSamitiMemberResponseDto>> GetAllMandalSamitiMemberByIdAsync(int id, CancellationToken ct)
        {
           return  await _context.Tbl_MandalSamitiMem.Where(x => x.MandalId == id).Select(b => new MandalSamitiMemberResponseDto
            {
               Id = b.Id,
               Name = b.Name,
               DesignationId = b.DesignationId,
               DesignationName = b.designation.Name,
               Occupation = b.Occupation,
               CategoryId = b.CategoryId,
               CategoryName = b.Category.Name,
               CasteId = b.CasteId,
               CasteName = b.Caste.CastName,
               Contact = b.Contact,
               Age = b.Age,
               MandalId = b.MandalId

            }).ToListAsync();
        }

        public async Task<List<MandalSamitiDesignationResponseDto>> GetMandalSamitiDesignationAsync()
        {
            return await _context.Tbl_MandalSamitiDesignation.Where(x => x.Status).Select(x => new MandalSamitiDesignationResponseDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
               
        }

        public Task<Tbl_MandalSamitiMem> GetMandalSamitiMemberByIdAsync(int id, CancellationToken ct)
        {
            return _context.Tbl_MandalSamitiMem.FirstOrDefaultAsync(x => x.Id == id, ct);

        }

        public void UpdateMandalSamitiMember(Tbl_MandalSamitiMem request)
        {
            _context.Tbl_MandalSamitiMem.Update(request);
           
        }
    }
}
