using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.Queries
{
    public class GetAllBoothVoterQueryHandler : IRequestHandler<GetAllBoothVoterQuery, List<BoothVoterResponseDto>>
    {
        private IBoothVoterRepository _repo;

        public GetAllBoothVoterQueryHandler(IBoothVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<BoothVoterResponseDto>> Handle(GetAllBoothVoterQuery query, CancellationToken cancellationtoken)
        {
            var res = await _repo.GetAllAsync();
            if (res == null)
            {
                throw new NotFoundException("Booth Voter Not Found");
            }
            return res;
        }
    }
}
