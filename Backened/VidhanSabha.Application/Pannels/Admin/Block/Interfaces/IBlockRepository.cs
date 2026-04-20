using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Block.Interfaces
{
    public interface IBlockRepository
    {
        Task<Tbl_Block?> GetByIdAsync(int id);
        Task<List<BlockResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_Block block, CancellationToken ct = default);
        int Update(Tbl_Block block);
        void Delete(Tbl_Block block);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
