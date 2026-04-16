using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Command
{
    public class CreateNewVoterCommandHandler : IRequestHandler<CreateNewVoterCommand, int>
    {
        private readonly INewVoterRepository _repo;

        public CreateNewVoterCommandHandler(INewVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateNewVoterCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;

            var data = Tbl_NewVoter.Create(
                req.BoothId, req.Name,req.FatherName, req.Mobile, req.CategoryId, req.CastId, req.DOB,req.Age,
                req.VoterId,  req.VillageId);


            return await _repo.AddAsync(data, cancellationToken);



        }
    }

}
