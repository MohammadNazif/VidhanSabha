using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Command
{
    public class CreateVidhanSabhaCommand : IRequest<int>
    {
       public CreateVidhanSabhaRequestDto Dto;
        public CreateVidhanSabhaCommand(CreateVidhanSabhaRequestDto dto)
        {
            Dto = dto;   
        }
       
 

    }
}
