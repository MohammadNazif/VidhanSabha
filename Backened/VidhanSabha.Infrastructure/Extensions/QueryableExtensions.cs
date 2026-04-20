using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
       
        public static async Task<PagedResult<TDto>> ToPagedResultAsync<TEntity, TDto>(
            this IQueryable<TEntity> query,
            BaseQueryParams queryParams,
            Expression<Func<TEntity, bool>>? searchPredicate,
            Expression<Func<TEntity, object>> defaultSort,
            Expression<Func<TEntity, TDto>> projection,
            CancellationToken ct = default)
        {
            // ── Search ──────────────────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm) && searchPredicate != null)
                query = query.Where(searchPredicate);

            // ── Count (before pagination, after filter) ─────────────────────
            var totalCount = await query.CountAsync(ct);

            // ── Sort ────────────────────────────────────────────────────────
            query = queryParams.IsDescending
                ? query.OrderByDescending(defaultSort)
                : query.OrderBy(defaultSort);

            // ── Paginate + Project ──────────────────────────────────────────
            var items = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(projection)
                .ToListAsync(ct);

            return new PagedResult<TDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }
    }
}
