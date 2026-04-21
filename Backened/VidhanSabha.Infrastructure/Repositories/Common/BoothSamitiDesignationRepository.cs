using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.BoothSamitiDesignation.DTOs;
using VidhanSabha.Application.Common.BoothSamitiDesignation.Interfaces;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    public class BoothSamitiDesignationRepository : IBoothSamitiDesignationRepository
    {
        private readonly DatabaseContext _context;

        public BoothSamitiDesignationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<DesignationDto>> GetAllAsync(CancellationToken ct = default)
        {
            try
            {
                return await _context.Tbl_BoothSamitiDesignations
                    .Select(x => new DesignationDto
                    {
                        Id = x.Id,
                        DesignationName = x.DesignationName
                    })
                    .ToListAsync(ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
