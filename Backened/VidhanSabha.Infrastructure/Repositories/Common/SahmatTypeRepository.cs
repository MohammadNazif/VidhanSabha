using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.SahmatAsahmatType.Interfaces;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    public class SahmatTypeRepository:BaseRepository<Tbl_SahmatType>,ISahmatTypeRepository
    {
        public SahmatTypeRepository(DatabaseContext context) : base(context) 
        {

        }
        public async Task<List<Tbl_SahmatType>> GetAllAsync(CancellationToken ct=default)
        {
            var data = await _context.Tbl_SahmatType.ToListAsync(ct);
            return data;
        }
    }
}
