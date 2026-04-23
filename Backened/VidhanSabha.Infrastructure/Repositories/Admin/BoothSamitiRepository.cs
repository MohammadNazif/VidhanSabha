using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Infrastructure.Persistence;

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
                await _context.Tbl_BoothSamitis.AddAsync(boothSamiti, ct);
                await _context.SaveChangesAsync(ct);
                return boothSamiti.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<BoothSamitiResponseDto>> GetAllAsync(CancellationToken ct = default)
        {
            try
            {
                return await _context.Tbl_BoothSamitis
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
    }
}
