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
    internal class UpdateDoubleVoterCommandHandler : IRequestHandler<UpdateDoubleVoterCommand, int>
    {
        private IDoubleVoterRepository _repo;

        public UpdateDoubleVoterCommandHandler(IDoubleVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdateDoubleVoterCommand request, CancellationToken cancellationtoken)
        {
            var req = request.Dto;
            var Double = await _repo.GetByIdAsync(req.Id);
            if (Double == null)
            {
                throw new NotFoundException("Double Voter Not Found");
            }
            Double.Update(
                req.BoothId, req.Name, req.FatherName,
                req.VoterId, req.PreviousAddress, req.CurrentAddress, req.Description, req.VillageId);
            return _repo.Update(Double);
        }
    }
}
