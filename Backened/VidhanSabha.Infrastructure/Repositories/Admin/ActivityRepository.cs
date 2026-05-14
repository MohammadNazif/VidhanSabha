using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Activity.Dtos;
using VidhanSabha.Application.Pannels.Admin.Activity.Interface;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class ActivityRepository : BaseRepository<Tbl_Activity>, IActivityRepository
    {
        public ActivityRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<int> AddAsync(Tbl_Activity entity, CancellationToken ct = default)
        {
            await _context.Tbl_Activity.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity.Id;
        }

        public async Task<Tbl_Activity?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await _context.Tbl_Activity
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status, ct);

        public async Task<PagedResult<ActivityResponseDto>> GetAllActiveAsync(ActivityQueryParams qp, CancellationToken ct = default)
        {
            // Base query: only active activities
            var query = _context.Tbl_Activity
                .Include(a => a.Images)
                .Where(a => a.Status)
                .AsNoTracking();

            // Optional search
            Expression<Func<Tbl_Activity, bool>>? search = null;
            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = a =>
                    a.Title.ToLower().Contains(term) ||
                    a.Description.ToLower().Contains(term);
            }

            // Return paginated result
            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: a => a.Date, // Latest first
                projection: a => new ActivityResponseDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    Date = a.Date,
                    YouTubeLink = a.YouTubeLink,
                    VideoPath = a.VideoPath,
                    ImagePaths = a.Images
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImagePath)
                        .ToList(),
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                },
                ct: ct
            );
        }

        public async Task UpdateAsync(Tbl_Activity entity, CancellationToken ct = default)
        {
            _context.Tbl_Activity.Update(entity);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
            await _context.Tbl_Activity.AnyAsync(x => x.Id == id && x.Status, ct);
    }
}