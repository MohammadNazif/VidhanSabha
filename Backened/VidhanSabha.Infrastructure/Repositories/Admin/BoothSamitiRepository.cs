using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
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

        public async Task<BoothSamitiResponseDto> GetAllAsync(int id, CancellationToken ct = default)
        {
            int boothId  =   await _context.Tbl_BoothSamitiMem.Where(x => x.Id == id).Select(b => b.BoothId).FirstOrDefaultAsync(ct);
            try
            {
                var members = await _context.Tbl_BoothSamitis
                    .Where(b => b.BoothIdMem == id)
                    .Include(x => x.Designation)
                    .Include(x => x.Category)
                    .Include(x => x.Caste)
                    .Select(x => new Application.Pannels.Admin.BoothSamiti.Dtos.Members
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

                var sanyojak = await _context.Tbl_BoothSanyojak
                    .Where(s => s.BoothId == boothId)
                    .Select(s => new SanyojakResponseDto
                    {
                        InchargeName = s.InchargeName,
                        Age = s.Age,
                        CastName = s.Cast.CastName,
                        CategoryName = s.Category.Name,
                        Designation = "Adhyakshya",
                        FatherName = s.FatherName,
                        EducationLevel = s.EducationLevel,
                        PhoneNumber = s.PhoneNumber,
                        Address = s.Address,
                        Profile = s.ProfileImagePath
                    })
                    .FirstOrDefaultAsync(ct);

                return new BoothSamitiResponseDto
                {
                    BoothSanyojak = sanyojak,
                    Members = members
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<BoothSamitiMemberExportRow>> GetAllMemberForExportAsync(BoothSamitiMemberFilter filter, CancellationToken ct = default)
        {
            int boothId = await _context.Tbl_BoothSamitiMem.Where(x => x.Id == filter.BoothMemId).Select(b => b.BoothId).FirstOrDefaultAsync(ct);
            var query = _context.Tbl_BoothSamitiMem.AsQueryable();  

            // Filter by BoothMemId if provided
            if (filter.BoothMemId.HasValue)
            {
                query = query.Where(b => b.Id == filter.BoothMemId.Value);
            }

            // TODO: Optional User filter if needed
            // Example: query = query.Where(b => b.CreatedByUserId == filter.UserId);

            var boothMemIds = await query.Select(b => b.Id).ToListAsync(ct);

            var membersQuery = _context.Tbl_BoothSamitis
                .Include(x => x.Designation)
                .Include(x => x.Category)
                .Include(x => x.Caste)
                .Where(b => boothMemIds.Contains(b.BoothIdMem));

            // Optional Search
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim();
                membersQuery = membersQuery.Where(m =>
                    EF.Functions.Like(m.Name, $"%{term}%") ||
                    EF.Functions.Like(m.Contact, $"%{term}%") ||
                    EF.Functions.Like(m.Occupation, $"%{term}%")
                );
            }

            var members = await membersQuery
                .Select(x => new BoothSamitiMemberExportRow
                {
                   
                    Name = x.Name,
                    Age = x.Age.ToString(),
                    Contact = x.Contact,
                    Category = x.Category.Name,
                    Caste = x.Caste.CastName,
                    Designation = x.Designation.DesignationName,
                    Occupation = x.Occupation
                })
                .ToListAsync(ct);

           
            
            var sanyojaks = await _context.Tbl_BoothSanyojak
                .Where(s =>s.BoothId == boothId)
                .Select(s => new BoothSamitiMemberExportRow
                {
                    
                    Name = s.InchargeName,
                    Age = s.Age.ToString(),
                    Contact = s.PhoneNumber,
                    Category = s.Category.Name,
                    Caste = s.Cast.CastName,
                    Designation = "Adhyakshya",
                  
                })
                .ToListAsync(ct);

            // Combine
            return sanyojaks.Concat(members).ToList();
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
                   Designation = "Adhyakshya",
                   // ✅ Booth details
                   BoothNo = m.Booth.BoothNumber,
                   PollingStation = m.Booth.PollingStationName,

                   TotalMember = m.TotalMembers,

                   // ✅ Booth Sanyojak
                   BoothAdhayaksh = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.InchargeName)
                        .FirstOrDefault(),

                   CastName = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.Cast.CastName)
                        .FirstOrDefault(),

                   CategoryName = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.Category.Name)
                        .FirstOrDefault(),

                   Age = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.Age)
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

        public async Task<List<BoothSamitiExportRow>> GetAllSamitiForExportAsync(BoothSamitiFilter filter, CancellationToken ct = default)
        {
            var query = _context.Tbl_BoothSamitiMem
                .AsNoTracking()
                .Where(b =>
                    (!filter.Id.HasValue || b.Id == filter.Id) &&
                    (b.Booth.Mandal.Status && b.Booth.Sector.Status) &&
                    (filter.UserId == null || b.UserId == filter.UserId || b.CreatedToUserId == filter.UserId || b.CreatedsectorUserId == filter.UserId) &&
                    (!filter.BoothId.HasValue || b.Booth.Id == filter.BoothId)
                );

            // Optional search
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim().ToLower();
                
            }

            var data = await query
                .OrderBy(m => m.Booth.BoothNumber)
                .Select(m => new BoothSamitiExportRow
                {
                    BoothNo = m.Booth.BoothNumber,
                    PollingStation = m.Booth.PollingStationName,
                    TotalMember = m.TotalMembers,
                    BoothAdhyaksh = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.InchargeName)
                        .FirstOrDefault(),
                    CastName = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.Cast.CastName)
                        .FirstOrDefault(),
                    CategoryName = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.Category.Name)
                        .FirstOrDefault(),
                    Age = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.Age)
                        .FirstOrDefault()
                        .ToString(),
                    Contact = _context.Tbl_BoothSanyojak
                        .Where(s => s.BoothId == m.BoothId)
                        .Select(s => s.PhoneNumber)
                        .FirstOrDefault(),
                    Village = _context.Tbl_BoothVillage
                        .Where(v => v.BoothId == m.BoothId)
                        .Select(v => v.Village.VillageName)
                        .FirstOrDefault(),
                    Designation = "Adhyakshya"
                })
                .ToListAsync(ct);

            return data;
        }

  
    }
}
