using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs;
using VidhanSabha.Application.Pannels.Admin.Pradhan.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    internal class PradhanRepository : BaseRepository<Tbl_Pradhan>, IPradhanRepository
    {
        private readonly DatabaseContext _context;
        public PradhanRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public Task<int> AddAsync(Tbl_Pradhan pradhan, CancellationToken ct = default)
        {
            try
            {
                _context.Tbl_Pradhan.AddAsync(pradhan);
                return _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public void Delete(Tbl_Pradhan pradhan)
        {
            try
            {
                _context.Tbl_Pradhan.Remove(pradhan);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public int Update(Tbl_Pradhan pradhan)
        {
            try
            {
                _context.Tbl_Pradhan.Update(pradhan);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }
        public async Task<Tbl_Pradhan?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tbl_Pradhan
                 .Include(p => p.Villages)
                 .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public Task<List<PradhanResponseDto>> GetAllAsync(int? id, CancellationToken ct=default)
        {
            try
            {
                return _context.Tbl_Pradhan
                    .Include(p => p.Villages)
                    .Select(p => new PradhanResponseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        DesignationId = p.DesignationId,
                        Contact = p.Contact,
                        Gender = p.Gender,
                        VillageId = p.Villages.Select(v => v.Id).ToList()
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }
    }
}
