using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
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
        public async Task<PagedResult<NewVoterResponseDto>> GetAllAsync(NewVoterQueryParams qp, CancellationToken ct = default)
        {
            var query = _context.Tbl_NewVoter
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) &&
                   (!qp.BoothId.HasValue || b.Booth.Id == qp.BoothId)&&
                   (!qp.VillageId.HasValue || b.Villages.Any(v => v.VillageId == qp.VillageId) &&
                   (!qp.CastId.HasValue ||  b.Cast.Id == qp.CastId))
                   
                   );

            Expression<Func<Tbl_NewVoter, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Booth.BoothNumber.Equals(Convert.ToInt32(term)) ||
                    b.Name.ToLower().Contains(term) ||
                    //b.Village.VillageName.ToLower().Contains(term) ||
                    b.VoterId.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Booth.BoothNumber,
               projection: m => new NewVoterResponseDto
               {
                   Id = m.Id,
                   SectorId=m.Booth.Sector.Id,
                   SectorName=m.Booth.Sector.SectorName,
                   SectorSanyojak=m.Booth.Sector.InchargeName,
                   BoothId = m.BoothId,
                   BoothNumber = m.Booth.BoothNumber,
                   BoothSanyojak=m.Booth.Sanyojak.InchargeName,
                   Name = m.Name,
                   FatherName = m.FatherName,
                   Mobile = m.Mobile,
                   CategoryId = m.CategoryId,
                   CategoryName = m.Category.Name,
                   DOB = m.DOB,
                   Age = m.Age,
                   CastId = m.CastId,
                   CastName = m.Cast.CastName,
                   VoterId = m.VoterId,
                   Villages = m.Villages.Select(v => new VillageResponseDtos
                   {
                       VillageId = v.VillageId,
                       VillageName = v.Village.VillageName
                   }).ToList(),
               },
               ct:ct
               );
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
