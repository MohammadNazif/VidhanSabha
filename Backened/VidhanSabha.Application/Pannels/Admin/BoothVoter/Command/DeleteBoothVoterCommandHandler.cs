using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.Command
{
    public class DeleteBoothVoterCommandHelper : IRequestHandler<DeleteBoothVoterCommand, int>
    {
        private IBoothVoterRepository _repo;

        public DeleteBoothVoterCommandHelper(IBoothVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteBoothVoterCommand request, CancellationToken cancellationtoken)
        {
            var boothvoter = await _repo.GetByIdAsync(request.Id);
            if (boothvoter == null)
            {
                throw new NotFoundException("Booth Voter Not Found");
            }
            boothvoter.Delete();
            return _repo.Update(boothvoter);
        }
    }
}
