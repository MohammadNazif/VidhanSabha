using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.MemberModulePermission.Dtos;
using VidhanSabha.Application.Common.NewFolder.Interface;

namespace VidhanSabha.Application.Common.MemberModulePermission.Query
{
    internal class getAllPermissionbyUseridQueryHandler : IRequestHandler<getAllPermissionbyUseridQuery, IReadOnlyList<MemberModulePermissionResDto>>
    {
        private IMemberModulePermissionRepository _repo;

        public getAllPermissionbyUseridQueryHandler(IMemberModulePermissionRepository repo)
        {
            _repo = repo;
        }
        public async Task<IReadOnlyList<MemberModulePermissionResDto>> Handle(getAllPermissionbyUseridQuery request, CancellationToken cancellationToken)
        {
             return await _repo.GetPermissionByUserIdAsync(request.UserId, cancellationToken);
        }   
    }
}
