using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Category.Interfaces;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    internal class Categories : ICategoryRepository
    {
        private DatabaseContext _context;

        public Categories(DatabaseContext context)
        {
            _context = context;   
        }
       
            public async Task<List<Tbl_Category>> GetAllAsync()
                             => await _context.Set<Tbl_Category>()
                             .OrderBy(c => c.Name)
                             .ToListAsync();

       
    }
}
