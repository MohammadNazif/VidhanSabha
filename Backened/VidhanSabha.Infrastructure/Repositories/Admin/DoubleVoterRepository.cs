using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
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
                .AsQueryable();

            var boothIds = qp.GetBoothIds();
            var SectorIds = qp.GetSectorIds();
            var villageIds = qp.GetVillageIds();

            // ✅ FIX 1: query = assign karo, sirf query.Where nahi
            query = query.Where(b =>
                (!qp.Id.HasValue || b.Id == qp.Id) && (b.Booth.Mandal.Status && b.Booth.Sector.Status) &&
                b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId || b.CreatedsectorUserId == qp.UserId || b.CreatedsectorUserId == qp.UserId) ;                 

            if (boothIds.Any())
                query = query.Where(b => boothIds.Contains(b.BoothId));

            //if (SectorIds.Any())
            //    query = query.Where(b => b.sec);

            if (villageIds.Any())
                query = query.Where(b => b.Villages.Any(v => villageIds.Contains(v.VillageId)));

            Expression<Func<Tbl_DoubleVoter, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Booth.BoothNumber.ToString().Contains(term) ||
                    b.Name.ToLower().Contains(term) ||
                    b.FatherName.ToLower().Contains(term) ||
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
        public async Task<List<doublevoterExportRow>> GetAllForExportAsync(
    DoubleVoterQueryParams qp,
    CancellationToken ct = default)
        {
            var query = _context.Tbl_DoubleVoter
                .AsNoTracking()
                .Include(d => d.Booth)
                    .ThenInclude(b => b.Sector)
                .Include(d => d.Booth)
                    .ThenInclude(b => b.Sanyojak)
                .Include(d => d.Villages)
                    .ThenInclude(v => v.Village)
                .AsQueryable();

            // ✅ Filters
            var boothIds = qp.GetBoothIds();
            var villageIds = qp.GetVillageIds();

            query = query.Where(b =>
                (!qp.Id.HasValue || b.Id == qp.Id) &&
                (b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId || b.CreatedsectorUserId == qp.UserId));

            if (boothIds?.Count > 0)
                query = query.Where(b => boothIds.Contains(b.BoothId));

            if (villageIds?.Count > 0)
                query = query.Where(b =>
                    b.Villages.Any(v => villageIds.Contains(v.VillageId)));

            // ✅ Search (safe)
            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim();

                query = query.Where(b =>
                    EF.Functions.Like(b.Name, $"%{term}%") ||
                    EF.Functions.Like(b.FatherName, $"%{term}%") ||
                    EF.Functions.Like(b.VoterId, $"%{term}%") ||
                    EF.Functions.Like(b.Booth.BoothNumber.ToString(), $"%{term}%")
                );
            }

            // ✅ Fetch minimal data
            var data = await query
                .OrderBy(b => b.Booth.BoothNumber)
                .Select(m => new
                {
                    m.Booth.BoothNumber,
                    m.Name,
                    m.FatherName,
                    m.VoterId,
                    m.CurrentAddress,
                    m.Description,

                    Villages = m.Villages
                        .Select(v => v.Village.VillageName)
                        .ToList()
                })
                .ToListAsync(ct);

            // ✅ Final mapping (string.Join here)
            return data.Select(m => new doublevoterExportRow
            {
                BoothNumber = m.BoothNumber,
                Village = string.Join(", ", m.Villages),
                VoterName = m.Name,
                FatherName = m.FatherName,
                VoterId = m.VoterId,
                CurrentAddress = m.CurrentAddress,
                Description = m.Description
            }).ToList();
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

