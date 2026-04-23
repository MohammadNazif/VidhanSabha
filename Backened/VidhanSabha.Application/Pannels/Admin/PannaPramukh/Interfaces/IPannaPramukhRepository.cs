using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces
{
    public interface IPannaPramukhRepository
    {
        Task<Tbl_PannaPramukh?> GetByIdAsync(int id);
        Task<PagedResult<PannaPramukhResponseDto>> GetAllAsync(PannaPramukhQueryParams qp, CancellationToken ct = default);
        Task<bool> PannaNumberExistsAsync(int boothId, int pannaNumber, int? excludeId = null, CancellationToken ct = default);
        Task AddAsync(Tbl_PannaPramukh panna, CancellationToken ct = default);
        int Update(Tbl_PannaPramukh panna);
        void Delete(Tbl_PannaPramukh panna);
        Task SaveAsync(CancellationToken ct = default);
    }
}
