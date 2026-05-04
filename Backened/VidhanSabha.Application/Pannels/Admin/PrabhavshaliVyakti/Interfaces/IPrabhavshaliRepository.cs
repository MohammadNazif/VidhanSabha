using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces
{
    public interface IPrabhavshaliRepository
    {
        Task<List<prabhavsaliExportRow>> GetExportByDesgIdAsync(PrabhavshaliQueryParams qp);
        Task<Tbl_PrabhavshaliVyakti?> GetByIdAsync(int id);
        Task<List<PrabhavshaliResponseDesinIdDto?>> GetByDesgIdAsync(int id,string userId);
        
        Task<PagedResult<PrabhavshaliResponseDto>> GetAllAsync(PrabhavshaliQueryParams qp, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_PrabhavshaliVyakti prabhav, CancellationToken ct = default);
        int Update(Tbl_PrabhavshaliVyakti prabhav);
        void Delete(Tbl_PrabhavshaliVyakti prabhav);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
