using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Village.Queries;
using VidhanSabha.Application.Pannels.Admin.Dashboard.Dtos;
using VidhanSabha.Application.Pannels.Admin.Dashboard.Interface;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.Entities.StatePrabhari;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    internal class AdminDashboardCount : BaseRepository<Tbl_Mandal>, IDashboard
    {
        public AdminDashboardCount(DatabaseContext context) : base(context)
        {
        }

        public async Task<DashboardCountsDto> GetDashboardCountsAsync(
        string userId)
        {
            var vidhanSabhaId = await _context.Tbl_StatePrabhari
                .Where(u => u.userId == userId)
                .Select(u => u.VidhansabhaId)
                .FirstOrDefaultAsync();

            var mandalIds = await _context.Tbl_Mandal
                .Where(m => m.VidhanId == vidhanSabhaId && m.Status)
                .Select(m => m.Id)
                .ToListAsync();

            var boothIds = await _context.Tbl_Booth
                .Where(b => mandalIds.Contains(b.MandalId) && b.Status)
                .Select(b => b.Id)
                .ToListAsync();

            return new DashboardCountsDto
            {
                Mandal = mandalIds.Count,
                Sector = await _context.Tbl_Sector
                                   .CountAsync(x => mandalIds.Contains(x.MandalId) && x.Status),
                Booth = boothIds.Count,
                PannaPramukh = await _context.Tbl_PannaPramukh
                                   .CountAsync(x => boothIds.Contains(x.BoothId) && x.Status && x.UserId == userId),

                Sahmat = await _context.Tbl_SahmatAsahmat.CountAsync(x => boothIds.Contains(x.BoothId) && x.TypeId == 1 && x.Status && x.UserId == userId),
                 Asahmat = await _context.Tbl_SahmatAsahmat.CountAsync(x => boothIds.Contains(x.BoothId) && x.TypeId == 2 && x.Status && x.UserId == userId),
                 Pravasi = await _context.Tbl_PravasiVoter.CountAsync(x => boothIds.Contains(x.BoothId) && x.Status && x.UserId == userId),
                 NewVoters = await _context.Tbl_NewVoter.CountAsync(x => boothIds.Contains(x.BoothId) && x.Status && x.UserId == userId),
                 DoubleVoter = await _context.Tbl_DoubleVoter.CountAsync(x => boothIds.Contains(x.BoothId) && x.Status && x.UserId == userId),
                 PrabhavshaliVyakti = await _context.Tbl_PrabhavshaliVyakti.CountAsync(x => boothIds.Contains(x.BoothId) && x.Status && x.UserId == userId),
                 Block = await _context.Tbl_Block.CountAsync( x => x.Status && x.UserId == userId
                 ),
                 BDC  = await _context.Tbl_Block.CountAsync(x => x.Status && x.UserId ==userId),
                InfluencerPerson = await _context.Tbl_Influencer.CountAsync(x => boothIds.Contains(x.BoothId) && x.Status && x.UserId == userId),
                Pradhan = await _context.Tbl_Pradhan.CountAsync(x => x.Status && x.UserId == userId),

                vidhanSabhaName = await _context.Tbl_StatePrabhari.Where(x => x.userId == userId).Select(b => b.Vidhansabha.VidhanSabhaName).FirstOrDefaultAsync(),

                  vidhanSabhaNumber = await _context.Tbl_StatePrabhari.Where(x => x.userId == userId).Select(b => b.Vidhansabha.VidhanSabhaNumber).FirstOrDefaultAsync()


            };
        }

        public async Task<StateDashboardCountsDto> GetStateDashboardCountsAsync(string userId)
        {
            return new StateDashboardCountsDto
            {
                VidhanSabha = await _context.Tbl_VidhanSabha.CountAsync(x => x.Status && x.UserId == userId),
                District = await _context.Tbl_DistrictWiseCount.CountAsync(x => x.Status && x.UserId == userId),
                Designation = await _context.Tbl_Designation.CountAsync(x => x.Status && x.UserId == userId),
                PradeshSamiti = await _context.Tbl_StateMembers.CountAsync(x => x.Status && x.UserId == userId && x.DesignationTypeId == 1 && x.Designation.Status),
                PradeshKaryarkarniSamiti = await _context.Tbl_StateMembers.CountAsync(x => x.Status && x.UserId == userId && x.DesignationTypeId == 2 && x.Designation.Status),
                StateId = await _context.Tbl_StatePrabhari.Where(x => x.userId == userId).Select(b => b.StateId).FirstOrDefaultAsync(),
            };

        }

        public async Task<BoothDashboardCountsDto> GetBoothDashboardCountsAsync(string userId)
        {
            return new BoothDashboardCountsDto
            {

                BoothId = await _context.Tbl_BoothSanyojak
        .Where(x => x.UserId == userId)
        .Select(b => b.BoothId)
         .FirstOrDefaultAsync(),
                PannaPramukh = await _context.Tbl_PannaPramukh
                                   .CountAsync(x =>x.Status && x.CreatedToUserId == userId),

                Sahmat = await _context.Tbl_SahmatAsahmat.CountAsync(x => x.Status && x.CreatedToUserId == userId && x.TypeId == 1),
                Asahmat = await _context.Tbl_SahmatAsahmat.CountAsync(x =>x.Status && x.CreatedToUserId == userId && x.TypeId == 2),
                Pravasi = await _context.Tbl_PravasiVoter.CountAsync(x => x.Status && x.CreatedToUserId == userId),
                NewVoters = await _context.Tbl_NewVoter.CountAsync(x => x.Status && x.CreatedToUserId == userId),
                DoubleVoter = await _context.Tbl_DoubleVoter.CountAsync(x => x.Status && x.CreatedToUserId == userId),
                PrabhavshaliVyakti = await _context.Tbl_PrabhavshaliVyakti.CountAsync(x => x.Status && x.CreatedToUserId == userId),
                BoothSamiti = await _context.Tbl_BoothSamitiMem.CountAsync(x => x.Status && x.CreatedToUserId == userId),
                VaristhNagrik = await _context.Tbl_SeniorDisabled.CountAsync(x => x.Status && x.CreatedToUserId == userId && x.TypeId == 1),
                Viklaang = await _context.Tbl_SeniorDisabled.CountAsync(x => x.Status && x.CreatedToUserId == userId && x.TypeId == 2),
                Post = await _context.Tbl_SocialMediaPost.CountAsync(x => x.Status),
                Activities = await _context.Tbl_Block.CountAsync(x => x.Status),
                BoothVoter = await _context.Tbl_BoothVoter.CountAsync(x => x.Status && x.CreatedToUserId == userId)
            };
           

        }

        public async Task<SectorDashboardCountsDto> GetSectorDashboardCountsAsync(string userId)
        {
            int sectorId;
            sectorId = await _context.Tbl_Sector
           .Where(x => x.UserId == userId)
          .Select(b => b.Id)
           .FirstOrDefaultAsync();
            return new SectorDashboardCountsDto
            {
               
               SectorId = sectorId,
                Booth = await _context.Tbl_Booth.CountAsync(x => x.Status && x.SectorId == sectorId),
                PannaPramukh = await _context.Tbl_PannaPramukh
                                   .CountAsync(x => x.Status && x.CreatedsectorUserId == userId),

                Sahmat = await _context.Tbl_SahmatAsahmat.CountAsync(x => x.Status && x.CreatedsectorUserId == userId && x.TypeId == 1),
                Asahmat = await _context.Tbl_SahmatAsahmat.CountAsync(x => x.Status && x.CreatedsectorUserId == userId && x.TypeId == 2),
                Pravasi = await _context.Tbl_PravasiVoter.CountAsync(x => x.Status && x.CreatedsectorUserId == userId),
                NewVoters = await _context.Tbl_NewVoter.CountAsync(x => x.Status && x.CreatedsectorUserId == userId),
                DoubleVoter = await _context.Tbl_DoubleVoter.CountAsync(x => x.Status && x.CreatedsectorUserId == userId),
                PrabhavshaliVyakti = await _context.Tbl_PrabhavshaliVyakti.CountAsync(x => x.Status && x.CreatedsectorUserId == userId),
                BoothSamiti = await _context.Tbl_BoothSamitiMem.CountAsync(x => x.Status && x.CreatedsectorUserId == userId),
                VaristhNagrik = await _context.Tbl_SeniorDisabled.CountAsync(x => x.Status && x.CreatedsectorUserId == userId && x.TypeId == 1),
                Viklaang = await _context.Tbl_SeniorDisabled.CountAsync(x => x.Status && x.CreatedsectorUserId == userId && x.TypeId == 2),
                Post = await _context.Tbl_SocialMediaPost.CountAsync(x => x.Status),
                Activities = await _context.Tbl_Block.CountAsync(x => x.Status),
                BoothVoter = await _context.Tbl_BoothVoter.CountAsync(x => x.Status && x.CreatedsectorUserId == userId)
            };


        }
    }
}

