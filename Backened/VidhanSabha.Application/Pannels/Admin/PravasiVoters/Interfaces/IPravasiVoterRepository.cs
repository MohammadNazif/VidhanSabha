using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces
{
    public interface IPravasiVoterRepository
    {
        Task<Tbl_PravasiVoter?> GetByIdAsync(int id);
        Task<PagedResult<PravasiVoterResponseDto>> GetAllAsync(PravasiQueryParams qp, CancellationToken ct = default);

        Task<List<PravasiVoterResponseDto>> GetAllForExportAsync(
    PravasiQueryParams qp, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_PravasiVoter pravasi, CancellationToken ct = default);
        int Update(Tbl_PravasiVoter pravasi);
        void Delete(Tbl_PravasiVoter pravasi);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
