using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Command
{
    public class CreatePrabhavCommandHandler : IRequestHandler<CreatePrabhavCommand, int>
    {
        private readonly IPrabhavshaliRepository _repo;

        public CreatePrabhavCommandHandler(IPrabhavshaliRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreatePrabhavCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;

            var data = Tbl_PrabhavshaliVyakti.Create(
                req.BoothId,req.DesignationId, req.Name, req.CategoryId, req.CastId, req.Mobile,req.Description,
                req.VillageId);

            return await _repo.AddAsync(data, cancellationToken);

        }
    }
}
