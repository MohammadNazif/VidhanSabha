using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Influencer.DTOs;
using VidhanSabha.Application.Pannels.Admin.Influencer.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Influencer.Queries
{
    public class GetAllInfluencerQueryHandler : IRequestHandler<GetAllInfluencerQuery, PagedResult<InfluencerResponseDto>>
    {
        private readonly IInfluencerRepository _repo;
        public GetAllInfluencerQueryHandler(IInfluencerRepository repo)
        {
            _repo = repo;
        }
        public async Task<PagedResult<InfluencerResponseDto>> Handle(GetAllInfluencerQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllAsync(request.QueryParams, cancellationToken);
            if (res == null)
            {
                throw new NotFoundException("Panna Pramukh Not Found");
            }

            return res;
        }
    }
}
