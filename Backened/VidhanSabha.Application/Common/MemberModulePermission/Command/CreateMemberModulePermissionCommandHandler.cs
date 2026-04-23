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
    internal class CreateMemberModulePermissionCommandHandler : IRequestHandler<CreateMemberModulePermissionCommand, int>
    {
        private IMemberModulePermissionRepository _repository;

        public CreateMemberModulePermissionCommandHandler(IMemberModulePermissionRepository repository)
        {

            _repository = repository;
        }
        public Task<int> Handle(CreateMemberModulePermissionCommand request, CancellationToken cancellationToken)
        {
            var req = request.Dto;
            var entities = req.Select(b => Tbl_MemberModulePermissions.Create(b.MemberId,
                b.Module,b.HasPermission
                )).ToList();

      
              return _repository.AddRangeAsync(entities);
        }
    }
}
   