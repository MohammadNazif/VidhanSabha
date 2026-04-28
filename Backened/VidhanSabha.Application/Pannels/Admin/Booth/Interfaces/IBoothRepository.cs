using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Booth.Dtos;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Interfaces
{
    public interface IBoothRepository
    {
        Task<Tbl_Booth?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Tbl_BoothSanyojak?> GetByBoothIdAsync(int boothId, CancellationToken ct);
        Task<List<BoothInchargeResponse>> GetInchargeByBoothIdAsync(int? boothId, CancellationToken ct);
        Task<PagedResult<BoothResponseDto>> GetAllAsync(BoothQueryParams qp,int? vidhanId, CancellationToken ct = default);
        Task<List<BoothNumberDto>> BoothNumberExistsAsync(string userId);
        Task<int> AddAsync(Tbl_Booth booth, CancellationToken ct = default);
        Task UpdateAsync(Tbl_Booth booth, CancellationToken ct);
        Task Delete(Tbl_Booth booth);
        Task SaveAsync(CancellationToken ct = default);
    }
}
