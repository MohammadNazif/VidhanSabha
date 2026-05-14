using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Commands
{
   public class CreateMandalCommand : IRequest<MandalResponseDto>
        {

          public CreateMandalRequestDto Dto { get; set; }
          public CreateMandalCommand(CreateMandalRequestDto dto)
            {
            Dto = dto;
            }
       }
}
