using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.NewFolder.Interface;

namespace VidhanSabha.Application.Common.MemberModulePermission.Command
{
    internal class CreateMemberModulePermissionCommandHandler : IRequestHandler<CreateMemberModulePermissionCommand, int>
    {
        private IMemberModulePermissionRepository _repository;

        public CreateMemberModulePermissionCommandHandler(IMemberModulePermissionRepository repository)
        {
            
            _repository = repository;
        }
        //public Task<int> Handle(CreateMemberModulePermissionCommand request, CancellationToken cancellationToken)
        //{
        //    return _repository.Add(request.Dto);
        //}
    }
}
