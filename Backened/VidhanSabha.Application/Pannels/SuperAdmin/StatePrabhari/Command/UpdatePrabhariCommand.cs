using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command
{
    public class UpdatePrabhariCommand : IRequest<int>
    {
        public UpdatePrabhariRequestDto Dto;
        public UpdatePrabhariCommand(UpdatePrabhariRequestDto dto)
        {
            Dto = dto;
        }
    }
}
