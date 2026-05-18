using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.Interfaces
{
    public interface IPradhanRepository
    {
        Task<Tbl_Pradhan?> GetByIdAsync(int id);
        Task<PagedResult<PradhanResponseDto>> GetAllAsync(PradhanQueryParams qp, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_Pradhan pradhan, CancellationToken ct = default);
        int Update(Tbl_Pradhan pradhan);
        void Delete(Tbl_Pradhan pradhan);

        Task<List<PradhanExportRow>> GetPradhanExportAsync(PradhanExportFilter qp);
    }
}
