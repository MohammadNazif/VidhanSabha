using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Interfaces
{
    public interface ISahmatAsahmatRepository
    {
        Task<List<sahmatExportRow>> GetAllForExportAsync(
      SahmatAsahmatQueryParams qp,
      CancellationToken ct = default);
        Task<Tbl_SahmatAsahmat?> GetByIdAsync(int id);
        Task<PagedResult<SahmatAsahmatResponseDto>> GetAllAsync(SahmatAsahmatQueryParams qp, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_SahmatAsahmat sahmatasahmat, CancellationToken ct = default);
        int Update(Tbl_SahmatAsahmat sahmatasahmat);
        void Delete(Tbl_SahmatAsahmat pravasi);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
