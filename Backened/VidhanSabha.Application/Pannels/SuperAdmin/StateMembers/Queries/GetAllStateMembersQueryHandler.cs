using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Queries;
using VidhanSabha.Application.Pannels.Auth.Interfaces;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.DTOs;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Interfaces;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Queries
{
    public class GetAllStateMembersQueryHandler : IRequestHandler<GetAllStateMembersQuery, PagedResult<StateMembersResponseDto>>
    {
        private IStateMembersRepository _repo;
        

        public GetAllStateMembersQueryHandler(IStateMembersRepository repo)
        {
            _repo = repo;
        }
        public async Task<PagedResult<StateMembersResponseDto>> Handle(GetAllStateMembersQuery request, CancellationToken cancellationToken)
        {

            var res = await _repo.GetAllAsync(request.QueryParams,request.SamitiType);
            if (res == null)
            {
                throw new NotFoundException("State Members Not Found");
            }

            return res;
        }
    }
}
