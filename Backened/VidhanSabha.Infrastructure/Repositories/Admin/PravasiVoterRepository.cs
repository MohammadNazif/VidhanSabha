using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class PravasiVoterRepository:BaseRepository<Tbl_PravasiVoter>,IPravasiVoterRepository
    {
        public PravasiVoterRepository(DatabaseContext context):base(context)
        {

        }
        public async Task<int> AddAsync(Tbl_PravasiVoter pravasi,CancellationToken ct=default)
        {
            try
            {
                await _context.Tbl_PravasiVoter.AddAsync(pravasi);
                return await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
            
            
        }

        public int Update(Tbl_PravasiVoter pravasi)
        {
            try
            {
                _context.Tbl_PravasiVoter.Update(pravasi);
                return _context.SaveChanges();
            }
            catch(Exception)
            {
                throw;
            }
            
        }
        public void Delete(Tbl_PravasiVoter pravasi)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PravasiVoterResponseDto>> GetAllAsync(int? boothId=null,CancellationToken ct=default)
        {
            var result = await _context.Tbl_PravasiVoter
                .Select(m => new PravasiVoterResponseDto
                {
                    Id = m.Id,
                    BoothId = m.BoothId,
                    BoothNumber=m.Booth.BoothNumber,
                    Name = m.Name,
                    Mobile = m.Mobile,
                    CategoryId = m.CategoryId,
                    CategoryName=m.Category.Name,
                    CastId = m.CastId,
                    CastName=m.Cast.CastName,
                    OccupationId = m.OccupationId,
                    VoterId = m.VoterId,
                    CurrentAddress = m.CurrentAddress,
                    Villages = m.Villages.Select(v => new VillageResponseDto
                    {
                        VillageId = v.VillageId,
                        VillageName = v.Village.VillageName
                    }).ToList(),
                }).ToListAsync();
            return result;
        }

        public async Task<Tbl_PravasiVoter?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_PravasiVoter
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
