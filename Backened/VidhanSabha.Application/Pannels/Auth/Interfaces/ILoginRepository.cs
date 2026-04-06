using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Auth;

namespace VidhanSabha.Application.Pannels.Auth.Interfaces
{
    public interface ILoginRepository
    {
        Task<Tbl_Login?> GetByMobileAsync(string mobileNumber);
        Task<Tbl_Login?> GetByUserIdAsync(int userId);
        Task AddAsync(Tbl_Login login);
        Task UpdateAsync(Tbl_Login login);
    }
}
