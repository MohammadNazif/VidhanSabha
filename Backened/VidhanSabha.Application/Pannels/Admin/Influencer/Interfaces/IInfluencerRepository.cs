using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Influencer.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Influencer.Interfaces
{
    public interface IInfluencerRepository
    {
        Task<Tbl_Influencer?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<List<InfluencerResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default);

        Task<int> AddAsync(Tbl_Influencer entity, CancellationToken ct = default);

        int Update(Tbl_Influencer entity);
        void Delete(Tbl_Influencer entity);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
