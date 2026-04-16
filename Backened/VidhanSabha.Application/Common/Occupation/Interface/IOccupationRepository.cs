using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.Occupation.Interface
{
    public interface IOccupationRepository
    {
        Task<List<Tbl_Occupation>> GetAllAsync();
    }
}
