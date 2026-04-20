using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.AdminDesignation.Interfaces;
using VidhanSabha.Application.Common.Party.Interfaces;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    public class AdminDesignationRepository : BaseRepository<Tbl_AdminDesignation>, IAdminDesignationRepository
    {
        public AdminDesignationRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<List<Tbl_AdminDesignation>> GetAllAsync(CancellationToken ct = default)
        {
            var data = await _context.Tbl_AdminDesignation.ToListAsync(ct);
            return data;
        }
    }
}
