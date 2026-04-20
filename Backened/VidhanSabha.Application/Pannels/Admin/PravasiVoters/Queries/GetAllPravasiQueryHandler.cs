using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.Queries
{
    public class GetAllPravasiQueryHandler:IRequestHandler<GetAllPravasiQuery, List<PravasiVoterResponseDto>>
    {
        private IPravasiVoterRepository _repo;

        public GetAllPravasiQueryHandler(IPravasiVoterRepository repo)
        {
            _repo = repo;
        }
            public async Task<List<PravasiVoterResponseDto>> Handle(GetAllPravasiQuery request, CancellationToken cancellationToken)
            {
                var res = await _repo.GetAllAsync();
                if(res==null)
                {
                    throw new NotFoundException("Pravasi Voter Not Found");
                }
                return res;
            }
    }
}
