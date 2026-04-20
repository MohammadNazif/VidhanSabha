using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Command
{
    public class UpdateVidhanSabhaCommand : IRequest<int>
    {
        public UpdatePrabhariRequestDto Dto { get; set; }
        public UpdateVidhanSabhaCommand(UpdatePrabhariRequestDto dto)
        {
            Dto = dto;
        }
    }
}
