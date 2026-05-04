using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.DTOs;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Commands
{
    public class CreateStateMembersCommand:IRequest<int>
    {
        public CreateStateMembersReqDto Dto{ get; set; }

        public string UserId { get; set; }
        public CreateStateMembersCommand(CreateStateMembersReqDto dto,string userId)
        {
            Dto = dto;
            UserId = userId;
        }
    }
}
