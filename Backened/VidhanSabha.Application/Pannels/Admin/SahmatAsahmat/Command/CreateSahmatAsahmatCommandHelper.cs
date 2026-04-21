using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Command
{
    public class CreateSahmatAsahmatCommandHandler : IRequestHandler<CreateSahmatAsahmatCommand, int>
    {
        private readonly ISahmatAsahmatRepository _repo;

        public CreateSahmatAsahmatCommandHandler(ISahmatAsahmatRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateSahmatAsahmatCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;

            var data = Tbl_SahmatAsahmat.Create(
                req.BoothId,req.TypeId,
                req.Name,req.Age,req.Mobile,req.PartyId,
                req.OccupationId,req.Reason,req.VoterId,
                req.VillageId
                );


            return await _repo.AddAsync(data, cancellationToken);



        }
    }
}
