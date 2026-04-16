using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Interfaces;
using VidhanSabha.Domain.Entities.SuperAdmin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.SuperAdmin
{
    internal class StatePrabhari : BaseRepository<Tbl_StatePrabhari>, IStatePrabhariRepository
    {
            public StatePrabhari(DatabaseContext context) : base(context)
            {
        }
        public async Task<int> AddAsync(Tbl_StatePrabhari prabhari, CancellationToken ct = default)
        {
             _context.Tbl_StatePrabhari.Add(prabhari);
            return await _context.SaveChangesAsync(ct);
        }

        public Task<bool> EmailExistsAsync(string email, int? excludeId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public  async Task<IReadOnlyList<StatePrabhariResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
             return await _context.Tbl_StatePrabhari.Select(b => new StatePrabhariResponseDto
            {
                userId = b.userId,
                 Id = b.Id,
                StateId = b.StateId,
                StateName = b.State.StateName,
                PrabhariName = b.PrabhariName,
                PrabhariEmail = b.PrabhariEmail,
                CategoryId = b.CategoryId,
                CastId = b.CastId,
                CategoryName = b.Category.Name,
                CastName = b.Cast.CastName,
                Education = b.Education,
                Profession = b.Profession,
                Gender = b.Gender,
                ContactNumber = b.ContactNumber,
                CurrentAddress = b.CurrentAddress,
            }).ToListAsync(ct);
        }

        public  async Task<Tbl_StatePrabhari?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Tbl_StatePrabhari.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        public Task SaveAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

         public async  Task<int> UpdateAsync(Tbl_StatePrabhari prabhari)
        {
            _context.Tbl_StatePrabhari.Update(prabhari);
            return await _context.SaveChangesAsync();
        }
    }
}
