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
    public class DeleteBlockCommandHandler : IRequestHandler<DeleteBlockCommand, int>
    {
        private IBlockRepository _repo;

        public DeleteBlockCommandHandler(IBlockRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteBlockCommand request, CancellationToken cancellationtoken)
        {
            var block = await _repo.GetByIdAsync(request.Id);

            if (block == null)
            {
                throw new NotFoundException("Block Not Found");
            }
            block.Delete();
            return _repo.Update(block);
        }
    }
}
