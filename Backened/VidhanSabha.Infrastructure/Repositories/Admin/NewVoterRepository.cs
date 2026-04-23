using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    internal class NewVoterRepository: BaseRepository<Tbl_NewVoter>, INewVoterRepository
    {
        public NewVoterRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<int> AddAsync(Tbl_NewVoter newvoter, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_NewVoter.AddAsync(newvoter);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(Tbl_NewVoter newvoter)
        {
            try
            {
                _context.Tbl_NewVoter.Update(newvoter);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_NewVoter newvoter)
        {
            throw new NotImplementedException();
        }

        public async Task<List<NewVoterResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default)
        {
            var result = await _context.Tbl_NewVoter
                .Select(m => new NewVoterResponseDto
                {
                    Id = m.Id,
                    BoothId = m.BoothId,
                    BoothNumber = m.Booth.BoothNumber,
                    Name = m.Name,
                    FatherName = m.FatherName,
                    Mobile = m.Mobile,
                    CategoryId = m.CategoryId,
                    CategoryName=m.Category.Name,
                    DOB=m.DOB,
                    Age=m.Age,
                    CastId = m.CastId,
                    CastName=m.Cast.CastName,
                    VoterId = m.VoterId,
                    Villages = m.Villages.Select(v => new VillageResponseDtos
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName
                    }).ToList(),
                }).ToListAsync();
            return result;
        }


        public async Task<Tbl_NewVoter?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_NewVoter
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
