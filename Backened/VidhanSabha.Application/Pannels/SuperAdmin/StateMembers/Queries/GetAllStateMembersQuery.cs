using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.DTOs;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Queries
{
    public record GetAllStateMembersQuery(StateMembersQueryParams QueryParams,int? SamitiType) : IRequest<PagedResult<StateMembersResponseDto>>
    {
      
       
    }
}
