using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Block.Queries
{
    public class GetAllBlockNameQueryHandler:IRequestHandler<GetAllBlockNameQuery,List<BlockNameResponse>>
    {
        private IBlockRepository _repo;

        public GetAllBlockNameQueryHandler(IBlockRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<BlockNameResponse>> Handle(GetAllBlockNameQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllBlockNameAsync(request.UserId);
            if (res == null)
            {
                throw new NotFoundException("Block Not Found");
            }
            return res;
        }

    }
}
