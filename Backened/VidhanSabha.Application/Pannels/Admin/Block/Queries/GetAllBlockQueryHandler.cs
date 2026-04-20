using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Queries;

namespace VidhanSabha.Application.Pannels.Admin.Block.Queries
{
    internal class GetAllBlockQueryHandler : IRequestHandler<GetAllBlockQuery, List<BlockResponseDto>>
    {
        private IBlockRepository _repo;

        public GetAllBlockQueryHandler(IBlockRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<BlockResponseDto>> Handle(GetAllBlockQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllAsync();
            if (res == null)
            {
                throw new NotFoundException("Block Not Found");
            }
            return res;
        }
    }
}
