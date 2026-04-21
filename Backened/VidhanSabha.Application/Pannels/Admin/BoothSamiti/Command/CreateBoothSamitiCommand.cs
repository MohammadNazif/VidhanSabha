using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Command
{
    public class CreateBoothSamitiCommand : IRequest<int>
    {
        public CreateBoothSamitiRequestDto Dto { get; set; }

        public CreateBoothSamitiCommand(CreateBoothSamitiRequestDto dto)
        {
            Dto = dto;
        }
    }
}
