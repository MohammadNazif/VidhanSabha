using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Queries;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.DTOs;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Queries
{
    public class GetAllSocialMediaPostQueryHandler : IRequestHandler<GetAllSocialMediaPostQuery, PagedResult<SocialMediaPostReponse>>
    {
        private ISocialMediaRepository _repo;

        public GetAllSocialMediaPostQueryHandler(ISocialMediaRepository repo)
        {
            _repo = repo;
        }
        public async Task<PagedResult<SocialMediaPostReponse>> Handle(GetAllSocialMediaPostQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllAsync(request.QueryParams);
            if (res == null)
            {
                throw new NotFoundException("SocialMedia Post Not Found");
            }
            return res;
        }

    }
}
