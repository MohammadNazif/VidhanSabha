
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

    public Task<Tbl_LoginCredential?> GetByMobileAsync(string mobileNumber)
    {
        return _context.Tbl_LoginCredential.FirstOrDefaultAsync( x => x.Mobile == mobileNumber);
         
    }

    public Task<Tbl_LoginCredential?> GetByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Tbl_LoginCredential login)
    {
        throw new NotImplementedException();
    }
}