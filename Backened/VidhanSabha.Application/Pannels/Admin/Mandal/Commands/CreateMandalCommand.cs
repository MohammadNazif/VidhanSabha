using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Commands
{
   public class CreateMandalCommand : IRequest<MandalResponseDto>
        {
            public int VidhanId { get; set; }
            public string Name { get; set; }

            public CreateMandalCommand(int vidhanId, string name)
            {
                VidhanId = 1;
                Name = name;
            }
       }
}
