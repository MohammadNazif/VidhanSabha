using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.DoubleVoter.Command
{
    internal class CreateDoubleVoterCommandHandler : IRequestHandler<CreateDoubleVoterCommand, int>
    {
        private readonly IDoubleVoterRepository _repo;

        public CreateDoubleVoterCommandHandler(IDoubleVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateDoubleVoterCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;

            var data = Tbl_DoubleVoter.Create(
                req.BoothId, req.Name, req.FatherName,
                req.VoterId,req.PreviousAddress, req.CurrentAddress,req.Description,request.UserId, req.VillageId);


            return await _repo.AddAsync(data, cancellationToken);

        }
    }
}
