using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces
{
    public interface IMandalRepository
    {
        Task<List<Tbl_Mandal>> GetAllAsync();
        Task<bool> ExistsByNameAsync(int vidhanId, string name);
        Task AddAsync(Tbl_Mandal mandal);
        Task<Tbl_Mandal> GetByIdAsync(int id);
        Task UpdateAsync(Tbl_Mandal mandal);
        
    }
}
