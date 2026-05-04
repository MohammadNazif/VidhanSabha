using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.DTOs;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Interfaces
{
    public interface IStateMembersRepository
    {
        Task<Tbl_StateMembers?> GetByIdAsync(int id);
        Task<PagedResult<StateMembersResponseDto>> GetAllAsync(StateMembersQueryParams qp,int? samitiType, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_StateMembers members, CancellationToken ct = default);
        int Update(Tbl_StateMembers members);
        void Delete(Tbl_StateMembers members);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
