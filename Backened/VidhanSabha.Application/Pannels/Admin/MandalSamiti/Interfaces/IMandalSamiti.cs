using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Dtos;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Interfaces
{
    public interface IMandalSamiti
    {

        Task<int> InsertMandalSamiti(Tbl_MandalSamiti request);

        void UpdateMandalSamiti(Tbl_MandalSamiti request);
        void UpdateMandalSamitiMember(Tbl_MandalSamitiMem request);

        void InsertMandalSamitiMember(Tbl_MandalSamitiMem request);
        Task<PagedResult<MandalSamitiResponseDto>> GetAllMandalSamitiAsync(MandalSamitiQueryParams qp,CancellationToken ct);

        Task<Tbl_MandalSamiti> GetMandalSamitiByIdAsync(int id, CancellationToken ct);
        Task<List<MandalSamitiDesignationResponseDto>> GetMandalSamitiDesignationAsync();

        Task<List<MandalSamitiMemberResponseDto>> GetAllMandalSamitiMemberByIdAsync(int id, CancellationToken ct);

        Task<Tbl_MandalSamitiMem> GetMandalSamitiMemberByIdAsync(int id, CancellationToken ct);

        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
