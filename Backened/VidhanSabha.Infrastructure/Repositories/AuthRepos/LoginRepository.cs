
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

    public async Task<Tbl_LoginCredential?> GetByMobileAsync(string mobileNumber)
    {
        try
        {
            return await _context.Tbl_LoginCredential.Where(x => x.Mobile == mobileNumber).Select(b => new Tbl_LoginCredential
            {
                UserId = b.UserId,
                Mobile = b.Mobile,
                Role = b.Role,
                Password = b.Password,
            }).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw;
        }
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

    public async Task AddRefreshTokenAsync(Tbl_RefreshToken token)
    {
        await _context.Tbl_RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<Tbl_RefreshToken?> GetRefreshTokenAsync(string token){
        try
            {
            return await _context.Tbl_RefreshTokens
                   .Include(r => r.User)
                   .FirstOrDefaultAsync(r => r.Token == token);
        }
        catch (Exception)
        {
            throw;
        }
    }
      
    public async Task DeleteExpiredTokensAsync(string userId) =>
        await _context.Tbl_RefreshTokens
            .Where(r => r.UserId == userId &&
                       (r.IsRevoked || r.ExpiresAt < DateTime.UtcNow))
            .ExecuteDeleteAsync();

    public async Task RevokeTokenAsync(string token) =>
        await _context.Tbl_RefreshTokens
            .Where(r => r.Token == token && !r.IsRevoked)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(r => r.IsRevoked, true));

    public async Task RevokeAllTokensAsync(string userId) =>
        await _context.Tbl_RefreshTokens
            .Where(r => r.UserId == userId && !r.IsRevoked)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(r => r.IsRevoked, true));
}
