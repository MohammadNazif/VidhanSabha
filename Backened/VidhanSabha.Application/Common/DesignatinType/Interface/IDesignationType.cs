using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.DesignatinType.Dto;

namespace VidhanSabha.Application.Common.DesignatinType.Interface
{
    public interface IDesignationType
    {
        Task<List<DesignationTypeResponseDto>> getAllAsync(CancellationToken ct = default);
    }
}
