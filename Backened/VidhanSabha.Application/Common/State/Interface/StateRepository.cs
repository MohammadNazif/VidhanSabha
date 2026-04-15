using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.State.Interface
{
    public interface IStateRepository
    {
        Task<List<Tbl_State>> getAllAsync(CancellationToken ct = default);
    }
}
