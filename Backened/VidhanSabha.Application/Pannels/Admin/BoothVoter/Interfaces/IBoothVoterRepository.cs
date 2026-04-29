using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.Interfaces
{
    public interface IBoothVoterRepository
    {
        Task<Tbl_BoothVoter?> GetByIdAsync(int id);
        Task<PagedResult<BoothVoterResponseDto>> GetAllAsync(BoothVoterQueryParams qp, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_BoothVoter boothvoter, CancellationToken ct = default);
        int Update(Tbl_BoothVoter boothvoter);
        void Delete(Tbl_BoothVoter boothvoter);
    }
}
