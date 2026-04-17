using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Queries
{
    public class GetAllNewVoterQueryHandler:IRequestHandler<GetAllNewVoterQuery,List<NewVoterResponseDto>>
    {
        private INewVoterRepository _repo;

        public GetAllNewVoterQueryHandler(INewVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<NewVoterResponseDto>> Handle(GetAllNewVoterQuery query,CancellationToken cancellationtoken)
        {
            var res = await _repo.GetAllAsync();
            if (res == null)
            {
                throw new NotFoundException("New Voter Not Found");
            }
            return res;
        }
    }
}
