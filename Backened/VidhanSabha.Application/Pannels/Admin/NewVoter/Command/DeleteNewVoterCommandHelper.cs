using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Command
{
    public class DeleteNewVoterCommandHelper:IRequestHandler<DeleteNewVoterCommand,int>
    {
        private INewVoterRepository _repo;

        public DeleteNewVoterCommandHelper(INewVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteNewVoterCommand request,CancellationToken cancellationtoken)
        {
            var newvoter = await _repo.GetByIdAsync(request.Id);
            if (newvoter == null)
            {
                throw new NotFoundException("New Voter Not Found");
            }
            newvoter.Delete();
            return _repo.Update(newvoter);  
            
        }
    }
}
