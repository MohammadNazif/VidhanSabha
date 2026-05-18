using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Interfaces;
using VidhanSabha.Domain.Entities.SuperAdmin;
using VidhanSabha.Domain.Enums;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.SuperAdmin
{
    internal class StatePrabhari : BaseRepository<Tbl_StatePrabhari>, IStatePrabhariRepository
    {
            public StatePrabhari(DatabaseContext context) : base(context)
            {
        }
        public async Task<int> AddAsync(Tbl_StatePrabhari prabhari, CancellationToken ct = default)
        {
            try
            {
                _context.Tbl_StatePrabhari.Add(prabhari);
                return await _context.SaveChangesAsync(ct);
            }
            catch(Exception)
            {

                throw;
            }
        }

        public Task<bool> EmailExistsAsync(string email, int? excludeId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public  async Task<IReadOnlyList<StatePrabhariResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
             return await _context.Tbl_StatePrabhari.Select(b => new StatePrabhariResponseDto
            {
                userId = b.userId,
                 Id = b.Id,
                StateId = b.StateId,
                StateName = b.State.StateName,
                PrabhariName = b.PrabhariName,
                PrabhariEmail = b.PrabhariEmail,
                CategoryId = b.CategoryId,
                CastId = b.CastId,
                CategoryName = b.Category.Name,
                CastName = b.Cast.CastName,
                Education = b.Education,
                Profession = b.Profession,
                Gender = b.Gender,
                ContactNumber = b.ContactNumber,
                CurrentAddress = b.CurrentAddress,
                Password = b.Login.Password
            }).ToListAsync(ct);
        }

        public  async Task<Tbl_StatePrabhari?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Tbl_StatePrabhari.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<PagedResult<StatePrabhariResponseDto>> GetByStateIdAsync(
       int stateId,
       string userId,
       QueryParams qp,
       CancellationToken ct = default)
        {
            var query = _context.Tbl_StatePrabhari
                .AsNoTracking()
                 .Include(x => x.Login) 
                .Where(m => m.StateId == stateId && m.Vidhansabha.Status &&
                            m.CreatedByUserId == userId &&
                            m.PrabhariRole == Domain.Enums.PrabhariRole.VidhanSabhaPrabhari);

            // Optional search
            Expression<Func<Tbl_StatePrabhari, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();

                search = x =>
                    x.PrabhariName.ToLower().Contains(term) ||
                    x.PrabhariEmail.ToLower().Contains(term) ||
                    x.ContactNumber.Contains(term) ||
                    x.Vidhansabha.VidhanSabhaName.ToLower().Contains(term) ||
                    x.Vidhansabha.district.DistrictName.ToLower().Contains(term);
            }

            return await query.ToPagedResultAsync(
                queryParams: qp,
                searchPredicate: search,
                defaultSort: x => x.Id,
                projection: x => new StatePrabhariResponseDto
                {
                    Id = x.Id,
                    userId = x.userId,
                    StateId = x.StateId,
                    VidhanSabhaId = x.VidhansabhaId,
                    DistrictId = x.Vidhansabha.DistrictId,
                    DistrictName = x.Vidhansabha.district.DistrictName,
                    VidhanSabhaName = x.Vidhansabha.VidhanSabhaName,
                    StateName = x.State != null ? x.State.StateName : string.Empty,
                    PrabhariName = x.PrabhariName,
                    PrabhariEmail = x.PrabhariEmail,
                    Gender = x.Gender,
                    ContactNumber = x.ContactNumber,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category != null ? x.Category.Name : string.Empty,
                    CastId = x.CastId,
                    CastName = x.Cast != null ? x.Cast.CastName : string.Empty,
                    Education = x.Education,
                    Profession = x.Profession,
                    CurrentAddress = x.CurrentAddress,
                    Password = x.Login != null ? x.Login.Password : null
                },
                ct: ct
            );
        }


        public async Task<StatePrabhariResponseDto?> GetProfileByUserIdAsync(
     string userId,
     string role,
     CancellationToken ct = default)
        {
            // Fetch login credentials first
            var loginData = await _context.Tbl_LoginCredential
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId, ct);

            // ================= STATE / VIDHANSABHA PRABHARI =================
            if (role == "StatePrabhari" || role == "VidhanSabhaPrabhari")
            {
                // Map role string to enum
                PrabhariRole requiredRole = role switch
                {
                    "StatePrabhari" => PrabhariRole.StatePrabhari,
                    "VidhanSabhaPrabhari" => PrabhariRole.VidhanSabhaPrabhari,
                    _ => throw new ArgumentException("Invalid role")
                };

                return await (
                    from sp in _context.Tbl_StatePrabhari

                    join s in _context.Tbl_State
                        on sp.StateId equals s.Id into stateJoin
                    from state in stateJoin.DefaultIfEmpty()

                    join vs in _context.Tbl_VidhanSabha
                        on sp.VidhansabhaId equals vs.Id into vsJoin
                    from vidhan in vsJoin.DefaultIfEmpty()

                    join d in _context.Tbl_District
                        on vidhan.DistrictId equals d.Id into districtJoin
                    from district in districtJoin.DefaultIfEmpty()

                    join c in _context.Tbl_Category
                        on sp.CategoryId equals c.Id into categoryJoin
                    from category in categoryJoin.DefaultIfEmpty()

                    join cast in _context.Tbl_Cast
                        on sp.CastId equals cast.Id into castJoin
                    from caste in castJoin.DefaultIfEmpty()

                    where sp.userId == userId
                          && sp.PrabhariRole == requiredRole

                    select new StatePrabhariResponseDto
                    {
                        Id = sp.Id,
                        userId = sp.userId,

                        StateId = sp.StateId,
                        StateName = state.StateName,

                        DistrictId = district.Id,
                        DistrictName = district.DistrictName,

                        VidhanSabhaId = sp.VidhansabhaId,
                        VidhanSabhaName = vidhan.VidhanSabhaName,
                        VidhanSabhaNumber = vidhan.VidhanSabhaNumber,

                        PrabhariName = sp.PrabhariName,
                        PrabhariEmail = sp.PrabhariEmail,
                        Gender = sp.Gender,
                        ContactNumber = sp.ContactNumber,

                        CategoryId = sp.CategoryId,
                        CategoryName = category.Name,

                        CastId = sp.CastId,
                        CastName = caste.CastName,

                        Education = sp.Education,
                        Profession = sp.Profession,
                        CurrentAddress = sp.CurrentAddress,
                        Password = loginData.Password
                    })
                    .FirstOrDefaultAsync(ct);
            }

            // ================= BOOTH SANYOJAK =================
            if (role == "BoothSanyojak")
            {
                return await (
                    from bs in _context.Tbl_BoothSanyojak

                    join c in _context.Tbl_Category
                        on bs.CategoryId equals c.Id into categoryJoin
                    from category in categoryJoin.DefaultIfEmpty()

                    join cast in _context.Tbl_Cast
                        on bs.CastId equals cast.Id into castJoin
                    from caste in castJoin.DefaultIfEmpty()

                    where bs.UserId == userId

                    select new StatePrabhariResponseDto
                    {
                        Id = bs.Id,
                        userId = bs.UserId,
                        BoothName = bs.Booth.PollingStationName,
                        BoothNumber = bs.Booth.BoothNumber,
                        PrabhariName = bs.InchargeName,
                        ContactNumber = bs.PhoneNumber,

                        CategoryId = bs.CategoryId,
                        CategoryName = category.Name,

                        CastId = bs.CastId,
                        CastName = caste.CastName,

                        Education = bs.EducationLevel,
                        CurrentAddress = bs.Address,
                        Profile = bs.ProfileImagePath,
                        Password = loginData.Password
                    })
                    .FirstOrDefaultAsync(ct);
            }

            // ================= SECTOR SANYOJAK =================
            if (role == "SectorSanyojak")
            {
                return await (
                    from ss in _context.Tbl_Sector

                    join c in _context.Tbl_Category
                        on ss.CategoryId equals c.Id into categoryJoin
                    from category in categoryJoin.DefaultIfEmpty()

                    join cast in _context.Tbl_Cast
                        on ss.CastId equals cast.Id into castJoin
                    from caste in castJoin.DefaultIfEmpty()

                    where ss.UserId == userId

                    select new StatePrabhariResponseDto
                    {
                        Id = ss.Id,
                        userId = ss.UserId,
                        SectorName = ss.SectorName,
                        PrabhariName = ss.InchargeName,
                        ContactNumber = ss.PhoneNumber,

                        CategoryId = ss.CategoryId,
                        CategoryName = category.Name,

                        CastId = ss.CastId,
                        CastName = caste.CastName,

                        Education = ss.EducationLevel,
                        CurrentAddress = ss.Address,
                        Profile = ss.ProfileImage,
                        Password = loginData.Password
                    })
                    .FirstOrDefaultAsync(ct);
            }

            return null;
        }

        public Task SaveAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

         public async  Task<int> UpdateAsync(Tbl_StatePrabhari prabhari)
        {
            _context.Tbl_StatePrabhari.Update(prabhari);
            return await _context.SaveChangesAsync();
        }
    }
}
