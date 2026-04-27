using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.DTOs;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Queries
{
    public class GetPlatformQueryHandler:IRequestHandler<GetPlatformQuery,List<SocialMediaPlatform>>
    {
        private ISocialMediaRepository _repo;

        public GetPlatformQueryHandler(ISocialMediaRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<SocialMediaPlatform>> Handle(GetPlatformQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetPlatformAsync();
            if (res == null)
            {
                throw new NotFoundException("Platform Not Found");
            }
            return res;
        }
    }
}
