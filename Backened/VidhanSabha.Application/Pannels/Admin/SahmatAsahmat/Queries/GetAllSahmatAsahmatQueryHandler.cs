using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Queries
{
    public class GetAllSahmatAsahmatQueryHandler:IRequestHandler<GetAllSahmatAsahmatQuery,PagedResult<SahmatAsahmatResponseDto>>
    {
        private ISahmatAsahmatRepository _repo;

        public GetAllSahmatAsahmatQueryHandler(ISahmatAsahmatRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<SahmatAsahmatResponseDto>> Handle(GetAllSahmatAsahmatQuery query,CancellationToken cancellationtoken)
        {
            var res = await _repo.GetAllAsync(query.QueryParams);
            if(res==null)
            {
                throw new NotFoundException("Sahmat/Asahmat Voter Not Found");
            }
            return res;
        }
    }
    
}
