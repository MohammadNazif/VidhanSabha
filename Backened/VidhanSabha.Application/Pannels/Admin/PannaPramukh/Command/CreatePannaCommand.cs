using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command
{
    public class CreatePannaCommand : IRequest<int>
    {
        public CreatePannaPramukhRequestDto Dto { get; set; }
        public string UserId { get; set; }

        public string Role { get; set; }

        public CreatePannaCommand(CreatePannaPramukhRequestDto createPannaPramukhRequestDto, string userId, string role )
        {
            Dto = createPannaPramukhRequestDto;
            UserId = userId;
            Role = role;
        }
    }
}
