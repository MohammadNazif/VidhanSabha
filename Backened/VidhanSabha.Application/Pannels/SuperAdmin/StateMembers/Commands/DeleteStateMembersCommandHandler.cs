using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Interfaces;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Commands
{
    public class DeleteStateMembersCommandHandler : IRequestHandler<DeleteStateMembersCommand, int>
    {
        private IStateMembersRepository _repo;

        public DeleteStateMembersCommandHandler(IStateMembersRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteStateMembersCommand request, CancellationToken cancellationToken)
        {
            var members = await _repo.GetByIdAsync(request.Id);

            if (members == null)
            {
                throw new NotFoundException("State Member Not Found");
            }

            members.Delete();

            return _repo.Update(members);
        }
    }
}
