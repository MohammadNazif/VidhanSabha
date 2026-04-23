using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.MemberModulePermission.Dtos
{
    public record MemberModulePermissionDto(
    int MemberId,
    string Module,
    bool HasPermission
  
);
}
