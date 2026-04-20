using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Pradhan;
using VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.Command
{
    public class CreatePradhanCommand:IRequest<int>
    {
        public CreatePradhanRequestDto Dto { get; set; }
        public CreatePradhanCommand(CreatePradhanRequestDto dto)
        {
            Dto = dto;
        }
    }
}
