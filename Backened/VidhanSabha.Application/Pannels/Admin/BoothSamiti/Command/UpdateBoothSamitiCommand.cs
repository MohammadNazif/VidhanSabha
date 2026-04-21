using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Command
{
    public class UpdateBoothSamitiCommand : IRequest<int>
    {
        public UpdateBoothSamitiRequestDto Dto { get; set; }

        public UpdateBoothSamitiCommand(UpdateBoothSamitiRequestDto dto)
        {
            Dto = dto;
        }
    }
}
