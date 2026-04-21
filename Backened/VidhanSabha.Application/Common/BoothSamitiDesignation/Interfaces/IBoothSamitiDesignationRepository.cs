using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.BoothSamitiDesignation.DTOs;

namespace VidhanSabha.Application.Common.BoothSamitiDesignation.Interfaces
{
    public interface IBoothSamitiDesignationRepository
    {
        Task<List<DesignationDto>> GetAllAsync(CancellationToken ct = default);
    }
}
