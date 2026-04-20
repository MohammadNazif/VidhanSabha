using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.DoubleVoter.Command
{
    public class DeleteDoubleVoterCommandHandler : IRequestHandler<DeleteDoubleVoterCommand, int>
    {
        private IDoubleVoterRepository _repo;

        public DeleteDoubleVoterCommandHandler(IDoubleVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteDoubleVoterCommand request, CancellationToken cancellationtoken)
        {
            var Double = await _repo.GetByIdAsync(request.Id);

            if (Double == null)
            {
                throw new NotFoundException("Double Voter Not Found");
            }
            Double.Delete();
            return _repo.Update(Double);
        }
    }
}
