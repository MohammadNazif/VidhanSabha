using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces
{
    public interface IMandalRepository
    {
        Task<PagedResult<MandalResponseDto>> GetAllAsync(MandalQueryParams qp,int? vidhanid,CancellationToken ct);
        Task<PagedResult<MandalFullDto>> GetAllCombinedMandalReports(MandalQueryParams qp,int? vidhansabhaId, CancellationToken ct);

        Task<PagedResult<MandalReportDto>> GetAllMandalReports(MandalQueryParams qp,int? vidhanId,CancellationToken ct);
        Task<bool> ExistsByNameAsync(int? vidhanId, string name);
        Task AddAsync(Tbl_Mandal mandal);
        Task<Tbl_Mandal> GetByIdAsync(int id);

        Task<int?> GetVidhansabhaIdByuserIdAsync(string userId);
        Task UpdateAsync(Tbl_Mandal mandal);
        
    }
}
