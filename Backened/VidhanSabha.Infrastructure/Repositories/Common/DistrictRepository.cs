using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Cast.Interfaces;
using VidhanSabha.Application.Common.District.Interfaces;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    public class DistrictRepository:BaseRepository<Tbl_District>, IDistrictRepository
    {
        public DistrictRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<List<Tbl_District>> GetDistrictsByIdAsync(int id)
        {
            return await _context.Tbl_District
                .Where(x => x.StateId == id)
                .ToListAsync();
        }
    }
    
}
