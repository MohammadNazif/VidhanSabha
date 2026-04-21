using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Category.Interfaces;
using VidhanSabha.Application.Common.Village.DTOs;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    public class Village : IVillageRepository
    {
        private DatabaseContext _context;

        public Village(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<List<Tbl_Village>> GetAllAsync()
        {
           return await _context.Set<Tbl_Village>().
                OrderBy(c => c.VillageName).
                ToListAsync();
        }

        public async Task<List<VillageResponseDtos>> GetAllVillageAsync()
        {
            return await _context.Set<Tbl_Village>()
                  
                  .Select(m => new VillageResponseDtos
                  {
                      Id = m.Id,
                      Name = m.VillageName
                  }
                  ).
                   ToListAsync();
        }

        public async Task<List<Tbl_Village>> GetAllByMandalIdAsync(int mandalId)
        {
            return await _context.Set<Tbl_Village>().
                  Where(m => m.MandalId == mandalId).
                   OrderBy(c => c.VillageName).
                   ToListAsync();
        }

        public async Task<List<VillageByBoothResponseDto>> GetAllByBoothIdAsync(int boothId)
        {
            return await _context.Set<Tbl_BoothVillage>().
                  Where(m => m.BoothId == boothId)
                  .Select(m => new VillageByBoothResponseDto
                  {
                      Id = m.VillageId,
                      Name = m.Village.VillageName
                  }
                  ).
                   ToListAsync();
        }
    }
}
