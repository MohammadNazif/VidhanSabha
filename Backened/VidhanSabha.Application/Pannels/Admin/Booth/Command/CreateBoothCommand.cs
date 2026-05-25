using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Command
{
    public record CreateBoothCommand : IRequest<int>
    {
        public BoothRequestDto Dto { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }

        public CreateBoothCommand(BoothRequestDto dto,string userId,string role)
        {
            Dto = dto;
            UserId = userId;
            Role = role;
        }
    }

}
