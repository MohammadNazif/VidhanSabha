using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Queries;

namespace VidhanSabha.Application.Pannels.Admin.DoubleVoter.Queries
{
    internal class GetAllDoubleVoterQueryHandler : IRequestHandler<GetAllDoubleVoterQuery, PagedResult<DoubleVoterResponseDto>>
    {
        private IDoubleVoterRepository _repo;

        public GetAllDoubleVoterQueryHandler(IDoubleVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<PagedResult<DoubleVoterResponseDto>> Handle(GetAllDoubleVoterQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllAsync(request.QueryParams, cancellationToken);
            if (res == null)
            {
                throw new NotFoundException("Double Voter Not Found");
            }
            return res;
        }
    }
}
