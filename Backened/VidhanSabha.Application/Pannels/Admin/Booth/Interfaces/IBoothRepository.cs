using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Interfaces
{
    public interface IBoothRepository
    {
        Task<Tbl_Booth?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<BoothResponseDto>> GetAllAsync(int? mandalId, int? sectorId, CancellationToken ct = default);
        //Task<Tbl_Booth?> GetByIdAsync(int id, CancellationToken ct);
        Task<List<BoothNumberDto>> BoothNumberExistsAsync();
        Task AddAsync(Tbl_Booth booth, CancellationToken ct = default);
        Task UpdateAsync(Tbl_Booth booth, CancellationToken ct);
        Task Delete(Tbl_Booth booth);
        Task SaveAsync(CancellationToken ct = default);
    }
}
