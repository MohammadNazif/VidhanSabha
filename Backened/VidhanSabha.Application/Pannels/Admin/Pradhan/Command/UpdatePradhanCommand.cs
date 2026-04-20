using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.Command
{
    public class UpdatePradhanCommand : IRequest<int>
    {
        public UpdatePradhanRequestDto Dto;
        public UpdatePradhanCommand(UpdatePradhanRequestDto dto)
        {
            Dto = dto;
        }
    }
}
