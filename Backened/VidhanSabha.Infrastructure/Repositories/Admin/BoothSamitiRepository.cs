using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    public class BoothSamitiRepository : IBoothSamitiRepository
    {
        private readonly DatabaseContext _context;

        public BoothSamitiRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Tbl_BoothSamiti boothSamiti, CancellationToken ct = default)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync(ct);
                await _context.Tbl_BoothSamitis.AddAsync(boothSamiti, ct);
                var boothId = boothSamiti.BoothIdMem;
                var mem = await _context.Tbl_BoothSamitiMem
                    .FirstOrDefaultAsync(x => x.Id == boothId, ct);
                if (mem != null)
                {
                    mem.Increment();
                }
                else
                {
                    //var newMem = Tbl_BoothSamitiMem.Create(boothId, boothSamiti.UserId);
                    //newMem.Increment();
                    //await _context.Tbl_BoothSamitiMem.AddAsync(newMem, ct);
                }

                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

                return boothSamiti.Id;
            }
            catch(Exception)
            {
                throw;
            }
        }
        public async Task<int> AddAsync(Tbl_BoothSamitiMem boothSamitimem, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_BoothSamitiMem.AddAsync(boothSamitimem, ct);
                await _context.SaveChangesAsync(ct);
                return boothSamitimem.Id;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BoothSamitiMemResponseDto?> GetBoothByIdAsync(
    int boothId,
    CancellationToken ct = default)
        {
            try
            {
                return await _context.Tbl_BoothSanyojak
                    .AsNoTracking()
                    .Where(m => m.BoothId == boothId)
                    .Select(m => new BoothSamitiMemResponseDto
                    {
                        Id = m.Id,
                        BoothId = m.BoothId,
                        BoothNo = m.Booth.BoothNumber,
                        PollingStation = m.Booth.PollingStationName,

                        TotalMember = 0,

                        // 🔥 Booth Sanyojak (separate table)
                        BoothAdhayaksh = _context.Tbl_BoothSanyojak
                            .Where(s => s.BoothId == m.BoothId)
                            .Select(s => s.InchargeName)
                            .FirstOrDefault(),

                        Contact = _context.Tbl_BoothSanyojak
                            .Where(s => s.BoothId == m.BoothId)
                            .Select(s => s.PhoneNumber)
                            .FirstOrDefault(),

                        // 🔥 Village (via BoothVillage + Village table)
                        Village = _context.Tbl_BoothVillage
                            .Where(v => v.BoothId == m.BoothId)
                            .Select(v => v.VillageId) // pehle ID
                            .FirstOrDefault() != 0
                            ? _context.Tbl_Village
                                .Where(x => x.Id ==
                                    _context.Tbl_BoothVillage
                                        .Where(v => v.BoothId == m.BoothId)
                                        .Select(v => v.VillageId)
                                        .FirstOrDefault()
                                )
                                .Select(x => x.VillageName)
                                .FirstOrDefault()
                            : null
                    })
                    .FirstOrDefaultAsync(ct);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<BoothSamitiResponseDto>> GetAllAsync(int id, CancellationToken ct = default)
        {
            try
            {
                return await _context.Tbl_BoothSamitis.Where(b => b.BoothIdMem == id)
              .Include(x => x.Designation)
             .Include(x => x.Category)
              .Include(x => x.Caste)
              .Select(x => new BoothSamitiResponseDto
           {
               Id = x.Id,
               Name = x.Name,

               CategoryId = x.CategoryId,
               CategoryName = x.Category.Name,

               CasteId = x.CasteId,
               CasteName = x.Caste.CastName,

               Age = x.Age,
               Contact = x.Contact,
               Occupation = x.Occupation,

               DesignationId = x.DesignationId,
               DesignationName = x.Designation.DesignationName
           })
           .ToListAsync(ct);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Tbl_BoothSamiti?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_BoothSamitis
                    .Include(x => x.Designation)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int Update(Tbl_BoothSamiti boothSamiti)
        {
            try
            {
                _context.Tbl_BoothSamitis.Update(boothSamiti);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Delete(Tbl_BoothSamiti boothSamiti)
        {
            try
            {
                var boothId = boothSamiti.BoothIdMem;
                var mem = _context.Tbl_BoothSamitiMem.FirstOrDefault(x => x.Id == boothId);
                if (mem != null)
                {
                    mem.Decrement();
                    _context.Tbl_BoothSamitiMem.Update(mem);
                }
                boothSamiti.Delete();
                _context.Tbl_BoothSamitis.Update(boothSamiti);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete Error: " + ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }

        public async Task<PagedResult<BoothSamitiMemResponseDto>> GetAllMem(BoothSamitiQueryParams qp, CancellationToken ct = default)
        {

            try
            {
        //        var vidhanSabhaId = await _context.Tbl_StatePrabhari
        //.Where(u => u.userId == qp.UserId)
        //.Select(u => u.VidhansabhaId)
        //.FirstOrDefaultAsync();
                var query = _context.Tbl_BoothSamitiMem
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) && (b.Booth.Mandal.Status && b.Booth.Sector.Status ) &&  (b.UserId == qp.UserId || b.CreatedToUserId == qp.UserId || b.CreatedsectorUserId == qp.UserId) &&
                   (!qp.BoothId.HasValue || b.Booth.Id == qp.BoothId)
                   );

                Expression<Func<Tbl_BoothSamitiMem, bool>>? search = null;

                if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
                {
                    var term = qp.SearchTerm.Trim().ToLower();
                    search = b =>
                        b.Booth.BoothNumber.Equals(Convert.ToInt32(term)) || b.Id.Equals(Convert.ToInt32(term));
                }


                return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Booth.BoothNumber,
               projection: m => new BoothSamitiMemResponseDto
               {
                   Id = m.Id,
                   BoothId = m.BoothId,
                   designationIds = m.Members.Select(x => x.DesignationId).Distinct().ToList(),
                   // ✅ Booth details
                   BoothNo = m.Booth.BoothNumber,
                   PollingStation = m.Booth.PollingStationName,

                   TotalMember = m.TotalMembers,

                   // ✅ Booth Sanyojak
                   BoothAdhayaksh = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.InchargeName)
                        .FirstOrDefault(),

                   Contact = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.PhoneNumber)
                        .FirstOrDefault(),

                   // ✅ Village
                   Village = _context.Tbl_BoothVillage
                        .Where(v => v.BoothId == m.BoothId)
                        .Select(v => v.Village.VillageName)
                        .FirstOrDefault()
               },
               ct: ct
               );
                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
