using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Interfaces
{
    public interface IBDCRepository
    {
        Task<Tbl_BDC?> GetByIdAsync(int id);
        Task<List<BDCResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_BDC bdc, CancellationToken ct = default);
        int Update(Tbl_BDC bdc);
        void Delete(Tbl_BDC bdc);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
