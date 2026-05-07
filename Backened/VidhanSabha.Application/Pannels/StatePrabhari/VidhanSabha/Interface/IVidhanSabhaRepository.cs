using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos;
using VidhanSabha.Domain.Entities.StatePrabhari;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Interface
{
    public interface IVidhanSabhaRepository
    {
        Task<int> AddAsync(Tbl_VidhanSabha vidhanSabha);
        Task<PagedResult<VidhanSabhaSatewiseResponseDto>> GetByIdAsync(
       vidhansabhaparams qp,
       int? districtId,
       CancellationToken ct = default);
        Task<VidhanSabhaSatewiseResponseDto?> GetByVidhanIdAsync(int vidhanId);

        Task<Tbl_VidhanSabha> GetVidhanSabhaByIdAsync(int id);
        Task<IEnumerable<Tbl_VidhanSabha>> GetAllAsync();
        Task<bool> UpdateAsync(Tbl_VidhanSabha vidhanSabha);

        Task<int> UpdateVidhanSabhaNameNumberAsync(Tbl_VidhanSabha vidhanSabha);
        Task<bool> DeleteAsync(int id);
    }
}
