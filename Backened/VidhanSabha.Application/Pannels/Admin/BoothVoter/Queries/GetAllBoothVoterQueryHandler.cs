using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.Queries
{
    public class GetAllBoothVoterQueryHandler : IRequestHandler<GetAllBoothVoterQuery, PagedResult<BoothVoterResponseDto>>
    {
        private IBoothVoterRepository _repo;

        public GetAllBoothVoterQueryHandler(IBoothVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<PagedResult<BoothVoterResponseDto>> Handle(GetAllBoothVoterQuery query, CancellationToken cancellationtoken)
        {
            var res = await _repo.GetAllAsync(query.QueryParams);
            if (res == null)
            {
                throw new NotFoundException("Booth Voter Not Found");
            }
            return res;
        }
    }
}
