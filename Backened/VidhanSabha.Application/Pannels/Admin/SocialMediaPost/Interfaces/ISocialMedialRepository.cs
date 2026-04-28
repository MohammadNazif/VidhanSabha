using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Interfaces
{
    public interface ISocialMediaRepository
    {
        Task<Tbl_SocialMediaPost?> GetByIdAsync(int id);
        Task<List<SocialMediaPlatform>> GetPlatformAsync();
        Task<PagedResult<SocialMediaPostReponse>> GetAllAsync(SocialMediaQueryParams qp, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_SocialMediaPost social, CancellationToken ct = default);
        int Update(Tbl_SocialMediaPost social);
        void Delete(Tbl_SocialMediaPost social);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
