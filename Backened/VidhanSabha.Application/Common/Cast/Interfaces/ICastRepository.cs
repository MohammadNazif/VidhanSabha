using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.Cast.Interfaces
{
    public interface ICastRepository
    {
        Task<List<Tbl_Cast>> GetAllCastByIdAsync(int id);
    }
}
