using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Village.DTOs;
using VidhanSabha.Application.Pannels.Admin.Influencer.DTOs;
using VidhanSabha.Application.Pannels.Admin.Influencer.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

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

    public async Task<List<InfluencerResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default)
    {
        return await _context.Tbl_Influencer
            .Select(x => new InfluencerResponseDto
            {
                Id = x.Id,
                // map fields here
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

            }).ToListAsync(ct);
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
