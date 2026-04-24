using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Common.MemberModulePermission.Command
{
    public class ModulePermissionRequirement : IAuthorizationRequirement
    {
        public ModulePermission Module { get; }

        public ModulePermissionRequirement(ModulePermission module)
        {
            Module = module;
        }
    }
}
