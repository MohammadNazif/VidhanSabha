using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Common.Village.DTOs;
using VidhanSabha.Application.Pannels.Admin.Influencer.DTOs;
using VidhanSabha.Application.Pannels.Admin.Influencer.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;
using static VidhanSabha.Application.Common.ExportPdfExcel.Dtos.InfluencerExportDef;

public class InfluencerRepository : BaseRepository<Tbl_Influencer>, IInfluencerRepository
{
    public InfluencerRepository(DatabaseContext context) : base(context)
    {
    }

    public async Task<int> AddAsync(Tbl_Influencer entity, CancellationToken ct = default)
    {
        try
        {
            await _context.Tbl_Influencer.AddAsync(entity, ct);
            return await _context.SaveChangesAsync(ct);

        }
        catch(Exception)
        {
            throw;
        }
        
        
    }

    public void Delete(Tbl_Influencer entity)
    {
        _context.Tbl_Influencer.Remove(entity);
    }

    public async Task<PagedResult<InfluencerResponseDto>> GetAllAsync(
        InfluencerQueryParams qp,
     CancellationToken ct = default)
       {
        //var vidhanSabhaId = await _context.Tbl_StatePrabhari
        //.Where(u => u.userId == qp.UserId)
        //.Select(u => u.VidhansabhaId)
        //.FirstOrDefaultAsync();
        var query = _context.Tbl_Influencer
            .AsNoTracking()
            .AsQueryable();

        // Filtering (optional)
        if (qp.BoothId.HasValue)
            query = query.Where(x => x.BoothId == qp.BoothId);

        //if (qp.CastId.HasValue)
        //    query = query.Where(x => x.CastId == qp.CastId);

        //if (qp.CategoryId.HasValue)
        //    query = query.Where(x => x.CategoryId == qp.CategoryId);


        query = query.Where(b => b.Status && (b.Booth.Mandal.Status && b.Booth.Sector.Status) && (b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId ));
        // Search
        Expression<Func<Tbl_Influencer, bool>>? search = null;

        if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
        {
            var term = qp.SearchTerm.Trim().ToLower();

            search = x =>
                x.Name.ToLower().Contains(term) ||
                x.Mobile.Contains(term) ||
                x.Cast.CastName.ToLower().Contains(term) ||
                x.Category.Name.ToLower().Contains(term) ||
                x.Booth.BoothNumber.ToString().Contains(term);
        }

        // Pagination + Projection
        return await query.ToPagedResultAsync(
            queryParams: qp,
            searchPredicate: search,
            defaultSort: x => x.Booth.BoothNumber,
            projection: x => new InfluencerResponseDto
            {
                Id = x.Id,
                BoothId = x.BoothId,
                BoothNumber = x.Booth.BoothNumber,
                Name = x.Name,
                Mobile = x.Mobile,
                CastId = x.CastId,
                CastName = x.Cast != null ? x.Cast.CastName : "",
                CategoryId = x.CategoryId,
                CategoryName = x.Category != null ? x.Category.Name : "",
                Description = x.Description,
                Villages = x.Villages.Select(v => new InfluencerVillageResponseDtos
                {
                    VillageIds = v.VillageId,
                    VillageName = v.Village.VillageName
                }).ToList()
            },
            ct: ct
        );
    }

    public async Task<List<InfluencerExportRow>> GetInfluencerExportAsync(InfluencerExportFilter qp)
    {
        var query = _context.Tbl_Influencer
            .AsNoTracking()
            .AsQueryable();

        if (qp.BoothId.HasValue)
            query = query.Where(x => x.BoothId == qp.BoothId);

        query = query.Where(b => b.Status && (b.Booth.Mandal.Status && b.Booth.Sector.Status) &&
                                 (b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId));

        if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
        {
            var term = qp.SearchTerm.Trim().ToLower();
            query = query.Where(x =>

                x.Name.ToLower().Contains(term) ||
                x.Mobile.Contains(term) ||
                x.Cast.CastName.ToLower().Contains(term) ||
                x.Category.Name.ToLower().Contains(term) ||
                x.Booth.BoothNumber.ToString().Contains(term)
            );
        }

        return await query.Select(x => new InfluencerExportRow
        {
            BoothNumber = x.Booth.BoothNumber,
            Name = x.Name,
            Mobile = x.Mobile,
            Cast = x.Cast != null ? x.Cast.CastName : "",
            Category = x.Category != null ? x.Category.Name : "",
            Villages = string.Join(", ", x.Villages.Select(v => v.Village.VillageName)),
            Description = x.Description
        }).ToListAsync();
    }
    public async Task<Tbl_Influencer?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Tbl_Influencer
            .Include(v=> v.Villages)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    //public async Task SaveAsync(CancellationToken ct = default)
    //{
    //    await _context.SaveChangesAsync(ct);
    //}

    public int Update(Tbl_Influencer entity)
    {
        _context.Tbl_Influencer.Update(entity);
        return _context.SaveChanges();
    }
}
