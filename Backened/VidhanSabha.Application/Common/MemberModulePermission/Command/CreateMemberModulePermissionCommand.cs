using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.MemberModulePermission.Dtos;

namespace VidhanSabha.Application.Common.MemberModulePermission.Command
{
    public class CreateMemberModulePermissionCommand : IRequest<int>
    {
        public MemberModulePermissionDto Dto;
        public CreateMemberModulePermissionCommand(MemberModulePermissionDto dto)
        {
            Dto = dto;
        }
    }
}
