using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Queries
{
    public record GetAllSocialMediaPostQuery(SocialMediaQueryParams QueryParams) : IRequest<PagedResult<SocialMediaPostReponse>>
    {
    }
}
