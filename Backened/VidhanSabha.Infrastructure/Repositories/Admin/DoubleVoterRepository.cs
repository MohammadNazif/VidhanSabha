using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class DoubleVoterRepository : BaseRepository<Tbl_PravasiVoter>, IDoubleVoterRepository
    {
        public DoubleVoterRepository(DatabaseContext context) : base(context)
        { }

        public async Task<int> AddAsync(Tbl_DoubleVoter doublevoter, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_DoubleVoter.AddAsync(doublevoter);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }


        }

        public int Update(Tbl_DoubleVoter doublevoter)
        {
            try
            {
                _context.Tbl_DoubleVoter.Update(doublevoter);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_DoubleVoter doublevoter)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DoubleVoterResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default)
        {
            var result = await _context.Tbl_DoubleVoter
                .Select(m => new DoubleVoterResponseDto
                {
                    Id = m.Id,
                    BoothId = m.BoothId,
                    BoothNumber = m.Booth.BoothNumber,
                    Name = m.Name,
                    FatherName=m.FatherName,
                    VoterId = m.VoterId,
                    PreviousAddress=m.PreviousAddress,
                    CurrentAddress = m.CurrentAddress,
                    Description=m.Description,
                    Villages = m.Villages.Select(v => new VillageResponseDtos
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName
                    }).ToList(),
                }).ToListAsync();
            return result;
        }

        public async Task<Tbl_DoubleVoter?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_DoubleVoter
                 .Include(p => p.Villages)
                 .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
