using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application   .Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces
{
    public interface IBoothSamitiRepository
    {
        Task<Tbl_BoothSamiti?> GetByIdAsync(int id);

        Task<BoothSamitiResponseDto> GetAllAsync(int id,CancellationToken ct = default);
        Task<List<BoothSamitiMemberExportRow>> GetAllMemberForExportAsync(BoothSamitiMemberFilter filter, CancellationToken ct = default);

        Task<List<BoothSamitiExportRow>> GetAllSamitiForExportAsync(BoothSamitiFilter filter, CancellationToken ct = default);
        Task<PagedResult<BoothSamitiMemResponseDto>> GetAllMem(BoothSamitiQueryParams qp, CancellationToken ct = default);

        Task<int> AddAsync(Tbl_BoothSamiti boothSamiti, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_BoothSamitiMem boothSamitimem, CancellationToken ct = default);
        Task<BoothSamitiMemResponseDto?> GetBoothByIdAsync(int boothId, CancellationToken ct = default);

        int Update(Tbl_BoothSamiti boothSamiti);

        void Delete(Tbl_BoothSamiti boothSamiti);
    }
}
