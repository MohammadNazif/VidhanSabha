using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Occupation.Interface;
using VidhanSabha.Application.Common.Party.Interfaces;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    internal class PartyRepository : BaseRepository<Tbl_Party>, IPartyRepository
    {
        public PartyRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<List<Tbl_Party>> GetAllAsync(CancellationToken ct = default)
        {
            var data = await _context.Tbl_Party.ToListAsync(ct);
            return data;
        }
    }
}
