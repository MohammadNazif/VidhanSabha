using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    internal class BoothVoterRepository : BaseRepository<Tbl_BoothVoter>, IBoothVoterRepository
    {
        public BoothVoterRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<int> AddAsync(Tbl_BoothVoter boothvoter, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_BoothVoter.AddAsync(boothvoter);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(Tbl_BoothVoter boothvoter)
        {
            try
            {
                _context.Tbl_BoothVoter.Update(boothvoter);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Delete(Tbl_BoothVoter boothvoter)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BoothVoterResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default)
        {
            var result = await _context.Tbl_BoothVoter
                .Select(m => new BoothVoterResponseDto
                {
                    Id = m.Id,
                    BoothId = m.BoothId,
                    BoothNumber = m.Booth.BoothNumber,
                    PollingStation = m.Booth.PollingStationName,
                    TotalVoter = m.TotalVoter,
                    Male = m.Male,
                    Female = m.Female,
                    Other = m.Other,
                    Villages = m.Villages.Select(v => new BoothVoterVillageResponseDtos
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName
                    }).ToList(),
                }).ToListAsync();
            return result;
        }


        public async Task<Tbl_BoothVoter?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_BoothVoter
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
