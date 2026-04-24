using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Common.MemberModulePermission.Dtos
{
    public record MemberModulePermissionDto(
    string MemberId,
    ModulePermission Module,
    bool HasPermission
  
   );

    public record updateMemberModulePermissionDto(

string MemberId,
ModulePermission Module,
bool HasPermission

);

    public class MemberModulePermissionResDto
    {
        public bool hasPermission { get; set; }

        public ModulePermission Module { get; set; }
 
    }
}
