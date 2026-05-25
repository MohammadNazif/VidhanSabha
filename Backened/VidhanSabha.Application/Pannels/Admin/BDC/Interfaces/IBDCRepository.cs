using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;
using VidhanSabha.Domain.Entities.Admin;
using static VidhanSabha.Application.Common.ExportPdfExcel.Dtos.BDCExportDef;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Interfaces
{
    public interface IBDCRepository
    {
        Task<Tbl_BDC?> GetByIdAsync(int id);
        Task<PagedResult<BDCResponseDto>> GetAllAsync(BDCQueryParams qp, CancellationToken ct = default);
        Task<List<BDCExportRow>> GetBDCExportAsync(BDCExportFilter qp);
        Task<int> AddAsync(Tbl_BDC bdc, CancellationToken ct = default);
        int Update(Tbl_BDC bdc);
        void Delete(Tbl_BDC bdc);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
