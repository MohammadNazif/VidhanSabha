using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.Category.Interfaces
{
    public interface  IVillageRepository
    {
        Task<List<Tbl_Village>> GetAllAsync();
        Task<List<Tbl_Village>> GetAllByMandalIdAsync(int mandalId);
    }
}
