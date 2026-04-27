using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Queries
{
    public class GetPlatformQuery:IRequest<List<SocialMediaPlatform>>
    {
    }
}
