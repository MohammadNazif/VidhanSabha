using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Command
{
    public class UpdateMandalSamitiMemberCommand : IRequest<int>
    {
        public UpdateMandalSamitiMemberRequestDto Dto { get; set; }
        public UpdateMandalSamitiMemberCommand(UpdateMandalSamitiMemberRequestDto dto)
        {
            Dto = dto;
        }
    }
}
