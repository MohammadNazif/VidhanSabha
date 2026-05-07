using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command
{
    public class CreatePrabhariCommand : IRequest<int>
    {
        public CreatePrabhariRequestDto Dto { get; private set; }
        public string UserId { get; private set; }
        public string Role { get; private set; }
        public CreatePrabhariCommand(CreatePrabhariRequestDto dto,string userId,string role)
        {
            Dto = dto;
            UserId = userId;
            Role = role;
        }
    }
}
