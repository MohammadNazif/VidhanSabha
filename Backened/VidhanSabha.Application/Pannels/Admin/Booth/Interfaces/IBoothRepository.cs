using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Booth.Dtos;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Interfaces
{
    public interface IBoothRepository
    {
        Task<List<BoothReportExportRow>> GetBoothReportExportAsync(
      BoothReportFilter filter,
      CancellationToken ct = default);
         Task<List<BoothExportRow>> GetAllForExportAsync(
         BoothQueryParams qp,
          CancellationToken ct = default);
        Task<Tbl_Booth?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Tbl_BoothSanyojak?> GetByBoothIdAsync(int boothId, CancellationToken ct);
        Task<List<BoothInchargeResponse>> GetInchargeByBoothIdAsync(int? boothId,string userId, CancellationToken ct);
        Task<PagedResult<BoothResponseDto>> GetAllAsync(BoothQueryParams qp,int? vidhanId, CancellationToken ct = default);
        Task<PagedResult<BoothReportsDto>> GetAllBoothReports(BoothQueryParams qp,string userId,CancellationToken ct=default);
        Task<List<BoothNumberDto>> BoothNumberExistsAsync(string userId);
        Task<List<BoothNumberDto>> BoothBysectorId(string userId);
        Task<string> GetUseridbyBoothId(int boothId);

        Task<string> GetSectorUseridbyBoothId(int boothId);

        Task<string> GetadminUseridbyUserId(int boothId);

        Task<string> GetadminUseridbySectorUserId(int sectorId);
        Task<int> AddAsync(Tbl_Booth booth, CancellationToken ct = default);
        Task UpdateAsync(Tbl_Booth booth, CancellationToken ct);
        Task Delete(Tbl_Booth booth);
        Task SaveAsync(CancellationToken ct = default);

        Task<string> GetSectorUseridbySectorId(int sectorId);
    }
}
