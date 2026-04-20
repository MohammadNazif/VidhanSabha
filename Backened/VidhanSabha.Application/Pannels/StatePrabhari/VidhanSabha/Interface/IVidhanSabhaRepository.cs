using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos;
using VidhanSabha.Domain.Entities.StatePrabhari;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Interface
{
    public interface IVidhanSabhaRepository
    {
        Task<int> AddAsync(Tbl_VidhanSabha vidhanSabha);
        Task<IReadOnlyList<VidhanSabhaSatewiseResponseDto?>> GetByIdAsync(int? stateId,int? districtId);
        Task<VidhanSabhaSatewiseResponseDto?> GetByVidhanIdAsync(int vidhanId);
        Task<IEnumerable<Tbl_VidhanSabha>> GetAllAsync();
        Task<bool> UpdateAsync(Tbl_VidhanSabha vidhanSabha);
        Task<bool> DeleteAsync(int id);
    }
}
