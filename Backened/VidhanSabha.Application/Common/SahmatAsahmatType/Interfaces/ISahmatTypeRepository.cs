using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.SahmatAsahmatType.Interfaces
{
    public interface ISahmatTypeRepository
    {
        Task<List<Tbl_SahmatType>> GetAllAsync();
    }
}
