using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
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

                var casteVoters = _context.Tbl_CasteVoter
                    .Where(x => x.CasteVoterId == boothvoter.Id)
                    .ToList();

                _context.Tbl_CasteVoter.RemoveRange(casteVoters);

                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(Tbl_BoothVoter boothvoter)
        {
            try
            {
                _context.Tbl_BoothVoter.Remove(boothvoter);
                _context.SaveChanges();
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<PagedResult<BoothVoterResponseDto>> GetAllAsync(BoothVoterQueryParams qp, CancellationToken ct = default)
        {
            var query = _context.Tbl_BoothVoter
    .AsNoTracking();

            var villageIds = qp.GetVillageIds();

            if(villageIds.Any())
            {
                query = query.Where(b => b.Villages.Any(v => villageIds.Contains(v.VillageId)));
            }

            query =  query.Where(b =>
        (!qp.Id.HasValue || b.Id == qp.Id) &&
        (b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId) &&
        (!qp.BoothId.HasValue || b.BoothId == qp.BoothId) &&
        (!qp.SectorId.HasValue || b.Booth.SectorId == qp.SectorId)
    );

            Expression<Func<Tbl_BoothVoter, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Booth.BoothNumber.Equals(Convert.ToInt32(term)) ||
                    b.Booth.PollingStationName.ToLower().Contains(term) ||
                    b.Villages.Select(v=>v.Village.VillageName).FirstOrDefault().ToLower().Contains(term)
                    ;
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Booth.BoothNumber,
               projection: m => new BoothVoterResponseDto
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
               },
               ct:ct
               );
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
