using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.Command
{
    public class CreateBoothVoterCommandHandler : IRequestHandler<CreateBoothVoterCommand, int>
    {
        private readonly IBoothVoterRepository _repo;

        public CreateBoothVoterCommandHandler(IBoothVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateBoothVoterCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;

            var data = Tbl_BoothVoter.Create(
                req.BoothId, req.TotalVoter, req.Male, req.Female, req.Other, req.VillageId);
            return await _repo.AddAsync(data, cancellationToken);
        }
    }
}
