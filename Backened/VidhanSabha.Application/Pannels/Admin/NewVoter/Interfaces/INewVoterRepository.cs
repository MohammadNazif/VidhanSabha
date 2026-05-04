using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces
{
    public interface INewVoterRepository
    {
        Task<List<newvoterExportRow>> GetAllForExportAsync(
            NewVoterQueryParams qp,
            CancellationToken ct = default);
        Task<Tbl_NewVoter?> GetByIdAsync(int id);
        Task<PagedResult<NewVoterResponseDto>> GetAllAsync(NewVoterQueryParams qp, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_NewVoter newvoter, CancellationToken ct = default);
        int Update(Tbl_NewVoter newvoter);
        void Delete(Tbl_NewVoter newvoter);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
