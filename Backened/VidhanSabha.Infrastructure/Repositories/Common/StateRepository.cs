using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.State.Interface;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    internal class StateRepository : BaseRepository<Tbl_State>, IStateRepository
    {
        public StateRepository(DatabaseContext context) : base(context)
        {
            
        }
        public async Task<List<Tbl_State>> getAllAsync(CancellationToken ct = default)
        {
           var data  = await _context.Tbl_State.ToListAsync(ct);
            return data;
        }
    }
}
