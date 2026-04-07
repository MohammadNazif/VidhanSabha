using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class MandalRepository : IMandalRepository
    {
        private readonly DatabaseContext _context;

        public MandalRepository(DatabaseContext context) => _context = context;

        public async Task<List<Tbl_Mandal>> GetAllAsync()
            => await _context.Set<Tbl_Mandal>()
                 .OrderBy(m => m.Name)
                 .ToListAsync();


        public async Task<Tbl_Mandal> GetByIdAsync(int id)
            => await _context.Set<Tbl_Mandal>()
                     .FirstOrDefaultAsync(x => x.Id == id);


        public async Task<bool> ExistsByNameAsync(int vidhanId, string name)
          => await _context.Set<Tbl_Mandal>().
                            Where(m => m.Status)
                           .AnyAsync(m => m.VidhanId == vidhanId
                                       && m.Name == name.Trim());

        public async Task AddAsync(Tbl_Mandal mandal)
        {
            await _context.Set<Tbl_Mandal>().AddAsync(mandal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tbl_Mandal mandal)
        {
             _context.Set<Tbl_Mandal>().Update(mandal);
            await _context.SaveChangesAsync();
        }
    }
}