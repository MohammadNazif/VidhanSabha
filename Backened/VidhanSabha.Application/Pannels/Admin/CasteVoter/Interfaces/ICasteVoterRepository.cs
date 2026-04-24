using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.CasteVoter.Interfaces
{
    public interface ICasteVoterRepository
    {
        Task<Tbl_CasteVoter?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<List<Tbl_CasteVoter>> GetByCasteVoterIdAsync(int CasteVoterId , CancellationToken ct = default);

        Task<PagedResult<CasteVoterResponseDto>> GetAllAsync(CancellationToken ct = default);

        Task<int> AddRangeAsync(List<Tbl_CasteVoter> castevoters, CancellationToken ct = default);

        Task<int> UpdateRangeAsync(List<Tbl_CasteVoter> castevoters, CancellationToken ct = default);

        Task<int> DeleteRangeAsync(List<Tbl_CasteVoter> castevoters, CancellationToken ct = default);

        Task<int> GetTotalVoterByCasteVoterIdAsync(int CasteVoterId , CancellationToken ct);
    }
}
