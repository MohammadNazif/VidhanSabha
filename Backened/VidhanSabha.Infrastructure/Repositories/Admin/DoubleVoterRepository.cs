using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
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

        public async Task<PagedResult<DoubleVoterResponseDto>> GetAllAsync
            (DoubleVoterQueryParams qp, CancellationToken ct = default)
        {

            var query = _context.Tbl_DoubleVoter
                .AsNoTracking()
                .Where(b =>
                    (!qp.Id.HasValue || b.Id == qp.Id) &&
                    (!qp.BoothId.HasValue || b.Booth.Id == qp.BoothId) &&
                    (!qp.SectorId.HasValue || b.Booth.Sector.Id == qp.SectorId) &&
                    (!qp.VillageId.HasValue || b.Villages.Any(v => v.VillageId == qp.VillageId))
                    );

            Expression<Func<Tbl_DoubleVoter, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Booth.BoothNumber.Equals(Convert.ToInt32(term)) ||
                    b.Name.ToLower().Contains(term) ||
                    //b.Village.VillageName.ToLower().Contains(term) ||
                    b.VoterId.ToLower().Contains(term);
            }
            try
            {
                return await query.ToPagedResultAsync(
                    queryParams: qp,
                    searchPredicate: search,
                    defaultSort: b => b.Booth.BoothNumber,
                    projection: m => new DoubleVoterResponseDto
                    {
                        Id = m.Id,
                        SectorId = m.Booth.Sector.Id,
                        Sector = m.Booth.Sector.SectorName,
                        SectorSanyojak = m.Booth.Sector.InchargeName,
                        BoothId = m.BoothId,
                        BoothNumber = m.Booth.BoothNumber,
                        BoothAdhyaksh = m.Booth.Sanyojak.InchargeName,
                        Name = m.Name,
                        FatherName = m.FatherName,
                        VoterId = m.VoterId,
                        PreviousAddress = m.PreviousAddress,
                        CurrentAddress = m.CurrentAddress,
                        Description = m.Description,
                        Villages = m.Villages.Select(v => new VillageResponseDtos
                        {
                            VillageId = v.VillageId,
                            VillageName = v.Village.VillageName
                        }).ToList(),
                    },
                    ct: ct
                );
            }
            catch (Exception)
            {
                throw;
            }
            
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

