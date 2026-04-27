using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Command
{
    public class CreateSocialMediaCommand : IRequest<int>
    {
        public CreateSocialMediaPostReqDto Dto { get; set; }
        public CreateSocialMediaCommand(CreateSocialMediaPostReqDto dto)
        {
            Dto = dto;
        }
    }
}
