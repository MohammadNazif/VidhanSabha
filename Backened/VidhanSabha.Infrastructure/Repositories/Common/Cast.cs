using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Cast.Interfaces;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    public class Cast : BaseRepository<Cast>, ICastRepository 
    {
        public Cast(DatabaseContext context) : base(context)
        {
            
        }

        public async Task<List<Tbl_Cast>> GetAllCastAsync()
        {
            return await _context.Tbl_Cast.OrderBy(b => b.CastName)
                .ToListAsync();
        }

        public async Task<List<Tbl_Cast>> GetAllCastByIdAsync(int id)
        {
            return await _context.Tbl_Cast
                .Where(x => x.CategoryId == id)
                .ToListAsync();
        }
    }
}
