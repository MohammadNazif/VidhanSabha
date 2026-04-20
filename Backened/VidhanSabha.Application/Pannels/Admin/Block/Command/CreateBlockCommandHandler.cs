using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Block.Command
{
    public class CreateBlockCommandHandler : IRequestHandler<CreateBlockCommand, int>
    {
        private readonly IBlockRepository _repo;

        public CreateBlockCommandHandler(IBlockRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateBlockCommand request, CancellationToken cancellationToken)
        {
            var req = request.Dto;

            var data = Tbl_Block.Create(
                req.BlockName, req.BlockPramukh,req.PartyId, req.Mobile,req.Address, req.CategoryId, req.CastId, req.OccupationId);

            return await _repo.AddAsync(data, cancellationToken);

        }
    }
}
