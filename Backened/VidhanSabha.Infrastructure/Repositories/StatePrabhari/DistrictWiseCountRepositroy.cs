using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Dtos;
using VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Interface;
using VidhanSabha.Domain.Entities.StatePrabhari;
using VidhanSabha.Domain.Entities.SuperAdmin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;
using static VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Dtos.DistrictWiseCount;

namespace VidhanSabha.Infrastructure.Repositories.StatePrabhari
{
    internal class DistrictWiseCountRepositroy : BaseRepository<Tbl_DistrictWiseCount>, IDistrictWiseCount
    {
        public DistrictWiseCountRepositroy(DatabaseContext context):base(context)
        {
            
        }
        public async Task<int> AddAsync(Tbl_DistrictWiseCount state, CancellationToken ct = default)
        {
            try
            {
                _context.Tbl_DistrictWiseCount.Add(state);

                return await _context.SaveChangesAsync(ct);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> ExistsAsync(int stateId, int? excludeId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Tbl_VidhansabhaStatewiseCount>> GetAllActiveAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<DistrictWiseCount.VidhansabhaDistrictResponseDto>> GetAllAsync(string? userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public  async  Task<IReadOnlyList<VidhansabhaDistrictResponseDto>?> GetByIdAsync(string userId, CancellationToken ct = default)
        {
             var res = await _context.Tbl_DistrictWiseCount.Where(e => e.UserId == userId)
                .Select(b =>  new VidhansabhaDistrictResponseDto
                {
                    Id = b.Id,
                    DistrictId = b.DistrictId,
                    DsitrictName = b.District.DistrictName,
                    VidhanSabhaCount = b.VidhansabhaCount,
                    RemainingCount = b.RemainingCount,

                })
                .ToListAsync(ct);

            return res;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Update(Tbl_DistrictWiseCount state)
        {
            throw new NotImplementedException();
        }
    }
}
