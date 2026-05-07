using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Interfaces;
using VidhanSabha.Domain.Entities.SuperAdmin;
using VidhanSabha.Infrastructure.Extensions;
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
            try
            {
                _context.Tbl_StatePrabhari.Add(prabhari);
                return await _context.SaveChangesAsync(ct);
            }
            catch(Exception)
            {

                throw;
            }
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

        public async Task<PagedResult<StatePrabhariResponseDto>> GetByStateIdAsync(
       int stateId,
       string userId,
       QueryParams qp,
       CancellationToken ct = default)
        {
            var query = _context.Tbl_StatePrabhari
                .AsNoTracking()
                .Where(m => m.StateId == stateId && m.Vidhansabha.Status &&
                            m.CreatedByUserId == userId &&
                            m.PrabhariRole == Domain.Enums.PrabhariRole.VidhanSabhaPrabhari);

            // Optional search
            Expression<Func<Tbl_StatePrabhari, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();

                search = x =>
                    x.PrabhariName.ToLower().Contains(term) ||
                    x.PrabhariEmail.ToLower().Contains(term) ||
                    x.ContactNumber.Contains(term) ||
                    x.Vidhansabha.VidhanSabhaName.ToLower().Contains(term) ||
                    x.Vidhansabha.district.DistrictName.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: x => x.Id,
                projection: x => new StatePrabhariResponseDto
                {
                    Id = x.Id,
                    userId = x.userId,
                    StateId = x.StateId,
                    VidhanSabhaId = x.VidhansabhaId,
                    DistrictId = x.Vidhansabha.DistrictId,
                    DistrictName = x.Vidhansabha.district.DistrictName,
                    VidhanSabhaName = x.Vidhansabha.VidhanSabhaName,
                    StateName = x.State != null ? x.State.StateName : string.Empty,
                    PrabhariName = x.PrabhariName,
                    PrabhariEmail = x.PrabhariEmail,
                    Gender = x.Gender,
                    ContactNumber = x.ContactNumber,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category != null ? x.Category.Name : string.Empty,
                    CastId = x.CastId,
                    CastName = x.Cast != null ? x.Cast.CastName : string.Empty,
                    Education = x.Education,
                    Profession = x.Profession,
                    CurrentAddress = x.CurrentAddress
                },
                ct: ct
            );
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
