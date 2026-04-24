using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.NewFolder.Interface;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.MemberModulePermission.Command
{
    internal class UpdateMemberModulePermissionCommandHandler : IRequestHandler<UpdateMemberModulePermissionCommand, int>
    {
        private IMemberModulePermissionRepository _perm;

        public UpdateMemberModulePermissionCommandHandler(IMemberModulePermissionRepository perm)
        {
            _perm = perm;
        }
        public async Task<int> Handle(UpdateMemberModulePermissionCommand request, CancellationToken cancellationToken)
        {
            var entities = request.Dto.Select(b =>
                Tbl_MemberModulePermissions.Create(b.MemberId, b.Module, b.HasPermission)
            ).ToList();

            return await _perm.UpdateRangeAsync(entities);
        }
    }
}
