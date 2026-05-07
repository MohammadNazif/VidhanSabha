using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Command
{
    public class UpdateVidhanSabhaNameNumberQuery : IRequest<int>
    {
        public UpdateVidhanSabhaRequestDto Dto { get; set; }
        public UpdateVidhanSabhaNameNumberQuery(UpdateVidhanSabhaRequestDto dto)
        {
            Dto = dto;
        }
    }
}
