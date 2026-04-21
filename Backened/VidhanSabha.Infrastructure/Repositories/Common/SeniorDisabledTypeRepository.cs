using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.SahmatAsahmatType.Interfaces;
using VidhanSabha.Application.Common.SeniorDisabledType.Interfaces;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    public class SeniorDisabledTypeRepository : BaseRepository<Tbl_SeniorDisabledType>, ISeniorDisabledTypeRepository
    {
        public SeniorDisabledTypeRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<List<Tbl_SeniorDisabledType>> GetAllAsync(CancellationToken ct = default)
        {
            var data = await _context.Tbl_SeniorDisabledType.ToListAsync(ct);
            return data;
        }
    }
}
