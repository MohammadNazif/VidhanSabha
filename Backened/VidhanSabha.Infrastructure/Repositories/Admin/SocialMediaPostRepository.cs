using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.DTOs;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class SocialMediaPostRepository:BaseRepository<Tbl_SocialMediaPost>,ISocialMediaRepository
    {
        public SocialMediaPostRepository(DatabaseContext context) : base(context)
        {

        }

        public async Task<int> AddAsync(Tbl_SocialMediaPost social, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_SocialMediaPost.AddAsync(social);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(Tbl_SocialMediaPost social)
        {
            try
            {
                _context.Tbl_SocialMediaPost.Update(social);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_SocialMediaPost social)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SocialMediaPlatform>> GetPlatformAsync()
        {
            try
            {
                var res = await _context.Tbl_SocialMediaPlatform
                    .Select(s => new SocialMediaPlatform
                    {
                        Id = s.Id,
                        PlatformName = s.Platform
                    }).ToListAsync();
                return res;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<Tbl_SocialMediaPost?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_SocialMediaPost
                 .Include(p => p.Booths)
                 .Include(p=>p.Platforms)
                 .Include(p=>p.Sectors)
                 .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PagedResult<SocialMediaPostReponse>> GetAllAsync(
            SocialMediaQueryParams qp,
            CancellationToken ct = default)
        {
            var query = _context.Tbl_SocialMediaPost
                .AsNoTracking()
                .AsQueryable();

            // Admin -> no filter
            if (qp.Role == PrabhariRole.VidhanSabhaPrabhari.ToString())
            {
                query = query.Where(x => x.UserId == qp.UserId);
            }
            else
            {
                // Booth user
                if (qp.BoothId.HasValue)
                {
                    query = query.Where(x =>
                        x.Booths.Any(b => b.BoothId == qp.BoothId.Value));
                }

                // Sector user
                if (qp.SectorId.HasValue)
                {
                    query = query.Where(x =>
                        x.Sectors.Any(s => s.SectorId == qp.SectorId.Value));
                }
            }

            // Optional search
            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.Title.ToLower().Contains(term) ||
                    x.Description.ToLower().Contains(term) ||
                    x.Platforms.Any(p =>
                        p.Platform.Platform.ToLower().Contains(term)));
            }

            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate : null,
                defaultSort: x => x.Id,
                projection: m => new SocialMediaPostReponse
                {
                    Id = m.Id,
                    Title = m.Title,
                    PostImagePath = m.PostImagePath,
                    Description = m.Description,

                    Platforms = m.Platforms.Select(v =>
                        new SocialMediaPlatformResponseDto
                        {
                            PlatformId = v.PlatformId,
                            PlatformName = v.Platform.Platform
                        }).ToList(),

                    Booths = m.Booths.Select(v =>
                        new SocialMediaBoothResponseDto
                        {
                            BoothId = v.BoothId,
                            BoothNo = v.Booth.BoothNumber
                        }).ToList(),

                    Sectors = m.Sectors.Select(v =>
                        new SocialMediaSectorResponseDto
                        {
                            SectorId = v.SectorId,
                            SectorName = v.Sector.SectorName
                        }).ToList()
                },
                ct: ct
            );
        }
    }
}
