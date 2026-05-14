using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Command
{
    public class CreateMandalSamitiMemberCommand : IRequest<int>
    {
        public CreateMandalSamitiMemberRequestDto Dto;
        public CreateMandalSamitiMemberCommand(CreateMandalSamitiMemberRequestDto dto)
        {
           Dto =dto;
        }
    }
}
