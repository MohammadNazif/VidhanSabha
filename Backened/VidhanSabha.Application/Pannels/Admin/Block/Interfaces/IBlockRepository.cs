using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.ExportPdfExcel.Dtos;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Domain.Entities.Admin;
using static VidhanSabha.Application.Common.ExportPdfExcel.Dtos.BlockExportDef;

namespace VidhanSabha.Application.Pannels.Admin.Block.Interfaces
{
    public interface IBlockRepository
    {
        Task<Tbl_Block?> GetByIdAsync(int id);
        Task<PagedResult<BlockResponseDto>> GetAllAsync(BlockQueryParams qp, CancellationToken ct = default);

        Task<List<BlockExportRow>> GetBlockExportAsync(BlockExportFilter qp);
        Task<List<BlockNameResponse>> GetAllBlockNameAsync(string? userId = null, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_Block block, CancellationToken ct = default);
        int Update(Tbl_Block block);
        void Delete(Tbl_Block block);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
