using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.SeniorDisabledType.Interfaces
{
    public interface ISeniorDisabledTypeRepository
    {
        Task<List<Tbl_SeniorDisabledType>> GetAllAsync();
    }
}
