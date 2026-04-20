using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Block.Command
{
    public class UpdateBlockCommandHandler : IRequestHandler<UpdateBlockCommand, int>
    {
        private IBlockRepository _repo;

        public UpdateBlockCommandHandler(IBlockRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdateBlockCommand request, CancellationToken cancellationtoken)
        {
            var dto = request.Dto;
            var block = await _repo.GetByIdAsync(dto.Id);
            if (block == null)
            {
                throw new NotFoundException("Block Not Found");
            }
            block.Update(dto.BlockName,
                dto.BlockPramukh,dto.PartyId, dto.Mobile,dto.Address, dto.CategoryId,
                dto.CastId, dto.OccupationId);
            return _repo.Update(block);
        }
    }
}
