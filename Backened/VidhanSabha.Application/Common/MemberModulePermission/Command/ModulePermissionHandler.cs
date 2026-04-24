using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VidhanSabha.Application.Common.NewFolder.Interface;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Common.MemberModulePermission.Command
{
    // VidhanSabha.Infrastructure/Authorization/ModulePermissionHandler.cs
    public class ModulePermissionHandler : AuthorizationHandler<ModulePermissionRequirement>
    {
        private readonly IMemberModulePermissionRepository _perm;

        public ModulePermissionHandler(IMemberModulePermissionRepository perm)
        {
            _perm = perm;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ModulePermissionRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

            // 1. No userId in token = reject
            if (string.IsNullOrEmpty(userId))
            {
                context.Fail();
                return;
            }

            // 2. Admin bypasses all permission checks
            if (role == PrabhariRole.VidhanSabhaPrabhari.ToString())
            {
                context.Succeed(requirement);
                return;
            }

            // 3. For other roles — check DB permission table
            var permissions = await _perm.GetPermissionByUserIdAsync(userId);

            var hasPermission = permissions
                .Any(p => p.Module == requirement.Module && p.hasPermission);

            if (hasPermission)
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}
