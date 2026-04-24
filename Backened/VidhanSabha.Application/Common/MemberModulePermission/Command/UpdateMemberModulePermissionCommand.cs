using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.MemberModulePermission.Dtos;

namespace VidhanSabha.Application.Common.MemberModulePermission.Command
{
    public class UpdateMemberModulePermissionCommand : IRequest<int>
    {
        public List<updateMemberModulePermissionDto> Dto { get; set; }

        public UpdateMemberModulePermissionCommand(List<updateMemberModulePermissionDto> dto)
        {
            Dto = dto;   
        }
    }
}
