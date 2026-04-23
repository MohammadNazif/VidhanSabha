
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Pannels.Auth.Interfaces;
using VidhanSabha.Domain.Entities.Auth;
using VidhanSabha.Infrastructure.Persistence;

public class LoginRepository : ILoginRepository
{
    private readonly DatabaseContext _context;

    public LoginRepository(DatabaseContext context)
    {
        _context = context;
    }

    public Task AddAsync(Tbl_LoginCredential login)
    {
        throw new NotImplementedException();
    }

    public  async Task<Tbl_LoginCredential?> GetByMobileAsync(string mobileNumber)
    {
       return await _context.Tbl_LoginCredential.Where(x => x.Mobile == mobileNumber).Select(b => new Tbl_LoginCredential
        {
            UserId = b.UserId,
            Mobile = b.Mobile,
            Role = b.Role,
            Password = b.Password,
        }).FirstOrDefaultAsync();
    }

    public async Task<int> GetBoothByUserIdAsync(string userId)
    {
        return await _context.Tbl_BoothSanyojak
            .Where(x => x.UserId == userId && x.Status)
            .Select(b => b.BoothId)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetSectorByUserIdAsync(string userId)
    {
        return await _context.Tbl_Sector
            .Where(x => x.UserId == userId && x.Status)
            .Select(b => b.Id)
            .FirstOrDefaultAsync();
    }
    public Task UpdateAsync(Tbl_LoginCredential login)
    {
        throw new NotImplementedException();
    }
}