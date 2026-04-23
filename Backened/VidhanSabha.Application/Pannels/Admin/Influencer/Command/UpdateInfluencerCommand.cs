using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Influencer.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Influencer.Command
{
    public class UpdateInfluencerCommand : IRequest<int>
    {
        public UpdateInfluencerReqDto Dto { get; set; }

        public UpdateInfluencerCommand(UpdateInfluencerReqDto dto)
        {
            Dto = dto;
        }
    }
}