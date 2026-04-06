
using Microsoft.EntityFrameworkCore;
using VidhanSabha.Application.Pannels.Auth.Interfaces;
using VidhanSabha.Domain.Entities.Auth;
using VidhanSabha.Infrastructure.Persistence;

public class LoginRepository  : ILoginRepository
{
    private readonly DatabaseContext _context;

    public LoginRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Tbl_Login?> GetByMobileAsync(string mobileNumber)
          => await _context.Set<Tbl_Login>()
                           .FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);

    public async Task<Tbl_Login?> GetByUserIdAsync(int userId)
        => await _context.Set<Tbl_Login>()
                         .FirstOrDefaultAsync(u => u.UserId == userId);

    public async Task AddAsync(Tbl_Login login)
    {
        await _context.Set<Tbl_Login>().AddAsync(login);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tbl_Login login)
    {
        _context.Set<Tbl_Login>().Update(login);
        await _context.SaveChangesAsync();
    }
}