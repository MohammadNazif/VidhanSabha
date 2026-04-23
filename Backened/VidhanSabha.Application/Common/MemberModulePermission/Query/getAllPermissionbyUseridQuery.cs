using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.MemberModulePermission.Dtos;

namespace VidhanSabha.Application.Common.MemberModulePermission.Query
{
    public class getAllPermissionbyUseridQuery : IRequest<IReadOnlyList<MemberModulePermissionResDto>>
    {
        public string UserId { get; set; }
        public getAllPermissionbyUseridQuery(string userId)
        {
            UserId = userId;
        }
    }
}
