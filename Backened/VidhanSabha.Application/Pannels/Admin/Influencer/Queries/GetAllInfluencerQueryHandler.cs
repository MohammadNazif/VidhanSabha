using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Influencer.DTOs;
using VidhanSabha.Application.Pannels.Admin.Influencer.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Influencer.Queries
{
    public class GetAllInfluencerQueryHandler : IRequestHandler<GetAllInfluencerQuery, List<InfluencerResponseDto>>
    {
        private readonly IInfluencerRepository _repo;
        public GetAllInfluencerQueryHandler(IInfluencerRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<InfluencerResponseDto>> Handle(GetAllInfluencerQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllAsync(null, cancellationToken);
            if (res == null)
            {
                throw new NotFoundException("Panna Pramukh Not Found");
            }

            return res;
        }
    }
}
