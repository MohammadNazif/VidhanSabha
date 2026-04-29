using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.DTOs;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Commands
{
    public class UpdateStateMembersCommand : IRequest<int>
    {
        public UpdateStateMembersReqDto Dto;
        public UpdateStateMembersCommand(UpdateStateMembersReqDto dto)
        {
            Dto = dto;
        }
    }
}
