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

        public async Task<PagedResult<MandalResponseDto>> GetAllAsync(MandalQueryParams qp,CancellationToken ct=default)
        {
            var query = _context.Tbl_Mandal
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id)
                   
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

        public async Task<PagedResult<CombinedMandalReportDto>> GetAllCombinedMandalReports(MandalQueryParams qp, CancellationToken ct = default)
        {
            var query = _context.Tbl_Mandal
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) &&
                   (!qp.SectorId.HasValue || b.Sector.Id == qp.SectorId) &&
                   (!qp.CastId.HasValue || b.Sector.Booth.Sanyojak.Cast.Id == qp.CastId) &&
                   (!qp.VillageId.HasValue || b.Sector.Booth.Villages.Any(v => v.VillageId == qp.VillageId))

                   );

            Expression<Func<Tbl_Mandal, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Name.ToLower().Contains(term) ||
                    b.Sector.Booth.PollingStationName.ToLower().Contains(term) ||
                    b.Sector.SectorName.ToLower().Contains(term) ||
                    b.Sector.InchargeName.ToLower().Contains(term) ||
                    b.Sector.Booth.Sanyojak.InchargeName.ToLower().Contains(term) ||
                    b.Sector.Booth.Sanyojak.PhoneNumber.ToLower().Contains(term) ||
                    b.Sector.Booth.Villages.Select(v => v.Village.VillageName).FirstOrDefault().ToLower().Contains(term) ||
                    b.Sector.Booth.Sanyojak.FatherName.ToLower().Contains(term) ||
                    b.Sector.Booth.Sanyojak.Age.ToString().Contains(term)||
                    b.Sector.Booth.Sanyojak.Cast.CastName.ToLower().Contains(term) ||
                    b.Sector.Booth.Sanyojak.Address.ToLower().Contains(term) ||
                    b.Sector.Booth.Sanyojak.EducationLevel.ToLower().Contains(term);
                //b.Village.VillageName.ToLower().Contains(term) ||
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Id,
               projection: m => new CombinedMandalReportDto
               {
                   MandalId = m.Id,
                   MandalName = m.Name,
                   SectorId = m.Sector.Id,
                   SectorName = m.Sector.SectorName,
                   SectorSanyojak = m.Sector.InchargeName,
                   SectorSanyojakContact = m.Sector.PhoneNumber,
                   BoothId=m.Sector.Booth.Id,
                   BoothNumber=m.Sector.Booth.BoothNumber,
                   PolllingStationName=m.Sector.Booth.PollingStationName,
                   BoothAdhyaksh = m.Sector.Booth.Sanyojak.InchargeName,
                   BoothAdhyakshContact = m.Sector.Booth.Sanyojak.PhoneNumber,
                   BoothAdhyakshVillageId = m.Sector.Booth.Villages.Select(v => v.VillageId).FirstOrDefault(),
                   BoothAdhyakshVillageName = m.Sector.Booth.Villages.Select(v=>v.Village.VillageName).FirstOrDefault(),
                   BoothAdhyakshFather = m.Sector.Booth.Sanyojak.FatherName,
                   BoothAdhyakshAge = m.Sector.Booth.Sanyojak.Age,
                   BoothAdhyakshCastId = m.Sector.Booth.Sanyojak.CastId,
                   BoothAdhyakshCastName = m.Sector.Booth.Sanyojak.Cast.CastName,
                   BoothAdhyakshAddress = m.Sector.Booth.Sanyojak.Address,
                   BoothAdhyakshEducation = m.Sector.Booth.Sanyojak.EducationLevel,
                   ProfileImagePath = m.Sector.Booth.Sanyojak.ProfileImagePath
               }, ct: ct);

        }

        public async Task<Tbl_Mandal> GetByIdAsync(int id)
            => await _context.Set<Tbl_Mandal>()
                     .FirstOrDefaultAsync(x => x.Id == id);


        public async Task<bool> ExistsByNameAsync(int vidhanId, string name)
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
    }
}