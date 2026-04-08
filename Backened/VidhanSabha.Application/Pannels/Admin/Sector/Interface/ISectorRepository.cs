using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Interface
{
    public interface ISectorRepository
    {
        Task<List<Tbl_Sector>> GetAllAsync();
        Task<List<Tbl_Sector>?> GetByMandalIdAsync(int id);

        Task<Tbl_Sector> GetByIdAsync(int id);
        Task AddAsync(Tbl_Sector sector);
        Task UpdateAsync(Tbl_Sector sector);
        Task DeleteAsync(Tbl_Sector sector);
    }
}
