using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Occupation.Interface;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    internal class OccupationRepository:BaseRepository<Tbl_Occupation>,IOccupationRepository
    {
        public OccupationRepository(DatabaseContext context):base(context)
        {

        }
        public async Task<List<Tbl_Occupation>> GetAllAsync(CancellationToken ct=default)
        {
            var data = await _context.Tbl_Occupation.ToListAsync(ct);
            return data;
        }
    }
    
}
