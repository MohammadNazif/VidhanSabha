using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class MandalRepository : IMandalRepository
    {
        private readonly DatabaseContext _context;

        public MandalRepository(DatabaseContext context) => _context = context;

        public async Task<PagedResult<MandalResponseDto>> GetAllAsync(MandalQueryParams qp,int? vidhanid,CancellationToken ct=default)
        {
            var query = _context.Tbl_Mandal
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) &&
                    (b.VidhanId == vidhanid)
                   );

            Expression<Func<Tbl_Mandal, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Name.ToLower().Contains(term);
                    //b.Village.VillageName.ToLower().Contains(term) ||
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Id,
               projection: m => new MandalResponseDto
               {
                   Id = m.Id,
                   VidhanId = m.VidhanId,
                   Name = m.Name,
                   Status = m.Status
               },ct:ct);


        }

        public async Task<PagedResult<MandalFullDto>> GetAllCombinedMandalReports(MandalQueryParams qp,CancellationToken ct = default)
        {
            var query = _context.Tbl_Mandal
                .AsNoTracking()
                .Where(m =>
                    (!qp.Id.HasValue || m.Id == qp.Id) &&
                    (!qp.SectorId.HasValue || m.Sectors.Any(s => s.Id == qp.SectorId)) &&
                    (!qp.CastId.HasValue || m.Sectors.Any(s =>
                        s.Booth != null &&
                        s.Booth.Sanyojak != null &&
                        s.Booth.Sanyojak.CastId == qp.CastId)) &&
                    (!qp.VillageId.HasValue || m.Sectors.Any(s =>
                        s.Booth != null &&
                        s.Booth.Villages.Any(v => v.VillageId == qp.VillageId)))
                );

            Expression<Func<Tbl_Mandal, bool>>? search = null;

            // 🔍 SEARCH (same style)
            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();

                search = m =>
                    m.Name.ToLower().Contains(term) ||

                    m.Sectors.Any(s =>
                        s.SectorName.ToLower().Contains(term) ||
                        s.InchargeName.ToLower().Contains(term) ||

                        (s.Booth != null &&
                         s.Booth.PollingStationName.ToLower().Contains(term)) ||

                        (s.Booth != null &&
                         s.Booth.Sanyojak != null &&
                         (
                            s.Booth.Sanyojak.InchargeName.ToLower().Contains(term) ||
                            s.Booth.Sanyojak.PhoneNumber.Contains(term)
                         )) ||

                        (s.Booth != null &&
                         s.Booth.Villages.Any(v =>
                            v.Village != null &&
                            v.Village.VillageName.ToLower().Contains(term)))
                    );
            }

            // 🚀 SAME PAGINATION METHOD (no manual Skip/Take)
            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: m => m.Id,
                projection: m => new MandalFullDto
                {
                    Id = m.Id,
                    Name = m.Name,

                    Sectors = m.Sectors.Select(s => new SectorDto
                    {
                        Id = s.Id,
                        SectorName = s.SectorName,
                        InchargeName = s.InchargeName,
                        PhoneNumber = s.PhoneNumber,

                        Booth = s.Booth != null ? new BoothDto
                        {
                            Id = s.Booth.Id,
                            BoothNumber = s.Booth.BoothNumber,
                            PollingStationName = s.Booth.PollingStationName,

                            Sanyojak = s.Booth != null && s.Booth.Sanyojak != null
                                ? new SanyojakDto
                                {
                                    Name = s.Booth.Sanyojak.InchargeName,
                                    Phone = s.Booth.Sanyojak.PhoneNumber,
                                    FatherName = s.Booth.Sanyojak.FatherName,
                                    Age = s.Booth.Sanyojak.Age,

                                    CastName = s.Booth.Sanyojak.Cast != null
                                        ? s.Booth.Sanyojak.Cast.CastName
                                        : null,

                                    Address = s.Booth.Sanyojak.Address,
                                    Education = s.Booth.Sanyojak.EducationLevel,
                                    ProfilePath = s.Booth.Sanyojak.ProfileImagePath
                                }
                                : null,

                            Villages = s.Booth.Villages != null
                                ? s.Booth.Villages.Select(v => new VillageDto
                                {
                                    Id = v.VillageId,
                                    Name = v.Village != null
                                        ? v.Village.VillageName
                                        : null
                                }).ToList()
                                : new List<VillageDto>()
                        }
                        : null
                    }).ToList()
                },
                ct: ct
            );
        }
        public async Task<Tbl_Mandal> GetByIdAsync(int id)
            => await _context.Set<Tbl_Mandal>()
                     .FirstOrDefaultAsync(x => x.Id == id);


        public async Task<bool> ExistsByNameAsync(int? vidhanId, string name)
          => await _context.Set<Tbl_Mandal>().
                            Where(m => m.Status)
                           .AnyAsync(m => m.VidhanId == vidhanId
                                       && m.Name == name.Trim());

        public async Task AddAsync(Tbl_Mandal mandal)
        {
            await _context.Set<Tbl_Mandal>().AddAsync(mandal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tbl_Mandal mandal)
        {
             _context.Set<Tbl_Mandal>().Update(mandal);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetVidhansabhaIdByuserIdAsync(string userId)
        {
            var data = await _context.Tbl_StatePrabhari
                .Where(v => v.userId == userId && v.Status)
                .Select(v => v.VidhansabhaId)
                .FirstOrDefaultAsync();

            return data;
        }
    }
}