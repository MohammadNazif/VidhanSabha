using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Interfaces;
using VidhanSabha.Domain.Entities.SuperAdmin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;
using static VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Dtos.VidhanshabhaStateWiseCount;

namespace VidhanSabha.Infrastructure.Repositories.SuperAdmin
{
    public class StateWiseVidhanSabhaCountRepository : BaseRepository<Tbl_VidhansabhaStatewiseCount>, IStateWiseVidhanSabhaCountRepository
    {
        public StateWiseVidhanSabhaCountRepository(DatabaseContext context) : base(context)
        {
            
        }

        public async Task<int> AddAsync(Tbl_VidhansabhaStatewiseCount state, CancellationToken ct = default)
        {
            try
            {
                _context.Tbl_VidhansabhaStatewiseCount.Add(state);
                return await _context.SaveChangesAsync(ct);
            }

            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> ExistsAsync(int stateId, int? excludeId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Tbl_VidhansabhaStatewiseCount>> GetAllActiveAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public  async Task<IReadOnlyList<VidhansabhaResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
             var res = await _context.Tbl_VidhansabhaStatewiseCount.Select(b => new VidhansabhaResponseDto
            {
                Id = b.Id,
                StateId = b.StateId,
                StateName = b.State.StateName,
                VidhanSabhaCount = b.VidhansabhaCount,
                RemainingCount = b.Remainingcount
            }).ToListAsync();
            return res;
        }

        public Task<Tbl_VidhansabhaStatewiseCount?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Update(Tbl_VidhansabhaStatewiseCount state)
        {
            throw new NotImplementedException();
        }
    }
}
