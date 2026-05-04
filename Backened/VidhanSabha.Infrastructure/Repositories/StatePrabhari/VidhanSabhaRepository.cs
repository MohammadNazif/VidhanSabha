using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Interface;
using VidhanSabha.Domain.Entities.StatePrabhari;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.StatePrabhari
{
    internal class VidhanSabhaRepository : BaseRepository<Tbl_VidhanSabha>, IVidhanSabhaRepository
    {
        public VidhanSabhaRepository(DatabaseContext context) : base(context)
        {
        }

            public async Task<int> AddAsync(Tbl_VidhanSabha vidhanSabha)
                {
                try
                {
                    await _context.Tbl_VidhanSabha.AddAsync(vidhanSabha);
                    await _context.SaveChangesAsync();
                    return vidhanSabha.Id;
                }
                catch (Exception)
                {
                    throw;
                }
            }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Tbl_VidhanSabha vidhanSabha)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Tbl_VidhanSabha>> IVidhanSabhaRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<VidhanSabhaSatewiseResponseDto?>> GetByIdAsync(vidhansabhaparams
         qp, int? districtId)
        {
            return await _context.Tbl_VidhanSabha.Where(x => x.UserId == qp.UserId && (districtId == null || x.DistrictId == districtId)).Select(b => new VidhanSabhaSatewiseResponseDto
            {
                Id = b.Id,
                DistrictId = b.DistrictId,  
                DistrictName = b.district.DistrictName,
                VidhanSabhaName = b.VidhanSabhaName,
                VidhanSabhaNumber = b.VidhanSabhaNumber,
                HasPrabhari = _context.Tbl_StatePrabhari.Any(p => p.VidhansabhaId == b.Id)
            }).
              OrderBy(x => x.DistrictName).ToListAsync();
        }

        public async Task<VidhanSabhaSatewiseResponseDto?> GetByVidhanIdAsync(int vidhanId)
        {
            return await _context.Tbl_VidhanSabha
                .Where(b => b.Id == vidhanId)
                .Select(b => new VidhanSabhaSatewiseResponseDto
                {
                    vidhanSabhaId = b.Id
                })
                .FirstOrDefaultAsync();
        }
    }
}
