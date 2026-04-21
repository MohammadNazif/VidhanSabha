using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BDC.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Command
{
    public class CreateBDCCommandHandler:IRequestHandler<CreateBDCCommand,int>
    {
        private IBDCRepository _repo;

        public CreateBDCCommandHandler(IBDCRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateBDCCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;

            var data = Tbl_BDC.Create(
                req.Block, req.Name,req.WardNumber, req.CategoryId, req.CastId,req.Age, 
                req.Mobile,req.PartyId,req.Education,
                req.VillageId);

            return await _repo.AddAsync(data, cancellationToken);

        }
    }
}
