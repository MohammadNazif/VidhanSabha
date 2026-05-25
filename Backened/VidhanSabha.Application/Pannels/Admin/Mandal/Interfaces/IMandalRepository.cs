using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos.VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces
{
    public interface IMandalRepository
    {
        Task<PagedResult<MandalResponseDto>> GetAllAsync(MandalQueryParams qp,int? vidhanid,CancellationToken ct);
        Task<PagedResult<MandalFullDto>> GetAllCombinedMandalReports(MandalQueryParams qp,int? vidhansabhaId, CancellationToken ct = default);

        Task<PagedResult<MandalReportDto>> GetAllMandalReports(MandalQueryParams qp,int? vidhanId,CancellationToken ct);
        Task<bool> ExistsByNameAsync(int? vidhanId, string name);

        Task<List<CombinedReportExportRow>> GetAllCombinedMandalReportsExp(
        CombinedReportFilter qp,
        int? vidhansabhaId,
        CancellationToken ct = default);
        Task AddAsync(Tbl_Mandal mandal);
        Task<Tbl_Mandal> GetByIdAsync(int id);
        Task<MandalSanyojakDto> GetMandalSanyojakByIdAsync(int id);

        Task<int?> GetVidhansabhaIdByuserIdAsync(string userId);

        Task<List<MandalReportExportRow>> GetAllMandalReportsForExport(
   MandalQueryParams qp,
   int? vidhanId);

        Task<List<MandalExportRow>> GetMandalExportAsync(
      MandalFilter filter,
      int? vidhanId,
      CancellationToken ct = default);
        Task UpdateAsync(Tbl_Mandal mandal);
        
    }
}
