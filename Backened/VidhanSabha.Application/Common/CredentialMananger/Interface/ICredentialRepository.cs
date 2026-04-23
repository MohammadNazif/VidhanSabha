using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Auth;

namespace VidhanSabha.Application.Common.CredentialMananger.Interface
{
    public interface ICredentialRepository
    {
        Task<Tbl_LoginCredential> InsertAsync(Tbl_LoginCredential login);
        Task<Tbl_LoginCredential> UpdateAsync(Tbl_LoginCredential login);
        Task<Tbl_LoginCredential?> GetByUserIdAsync(string userId);  // ← add
        Task<int> SoftDeleteAsync(Tbl_LoginCredential data);
    }
}
