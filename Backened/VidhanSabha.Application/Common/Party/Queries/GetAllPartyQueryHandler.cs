using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Occupation.DTOs;
using VidhanSabha.Application.Common.Party.DTOs;
using VidhanSabha.Application.Common.Party.Interfaces;
using VidhanSabha.Application.Exceptions;

namespace VidhanSabha.Application.Common.Party.Queries
{
    public class GetAllPartyQueryHandler : IRequestHandler<GetAllPartyQuery,List<PartyResponseDto>>
    {
        private IPartyRepository _repo;

        public GetAllPartyQueryHandler(IPartyRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<PartyResponseDto>> Handle(GetAllPartyQuery query,CancellationToken cancellationtoken)
        {
            var res = await _repo.GetAllAsync();
            if(res==null)
            {
                throw new NotFoundException("Party Not Found");
            }
            return res.Select(x => new PartyResponseDto
            {
                Id = x.Id,
                Party = x.Party,
            }).ToList();
        }
    }
}
