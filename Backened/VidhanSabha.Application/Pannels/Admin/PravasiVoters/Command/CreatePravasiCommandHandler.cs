using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command
{
    public class CreatePravasiCommandHandler:IRequestHandler<CreatePravasiCommand,int>
    {
        private readonly IPravasiVoterRepository _repo;

        public CreatePravasiCommandHandler(IPravasiVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreatePravasiCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;

            var data =  Tbl_PravasiVoter.Create(
                req.BoothId, req.Name, req.Mobile, req.CategoryId, req.CastId, req.OccupationId,
                req.VoterId, req.CurrentAddress, req.VillageId);

            
            return  await _repo.AddAsync(data,cancellationToken);
            


        }
    }
}
