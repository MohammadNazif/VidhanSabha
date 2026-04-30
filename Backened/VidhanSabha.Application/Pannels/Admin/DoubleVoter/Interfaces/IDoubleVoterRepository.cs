using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.DoubleVoter.Interfaces
{
    public interface IDoubleVoterRepository
    {

        Task<List<doublevoterExportRow>> GetAllForExportAsync(
      DoubleVoterQueryParams qp,
      CancellationToken ct = default);
        Task<Tbl_DoubleVoter?> GetByIdAsync(int id);
        Task<PagedResult<DoubleVoterResponseDto>> GetAllAsync(DoubleVoterQueryParams qp, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_DoubleVoter Double, CancellationToken ct = default);
        int Update(Tbl_DoubleVoter Double);
        void Delete(Tbl_DoubleVoter Double);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
