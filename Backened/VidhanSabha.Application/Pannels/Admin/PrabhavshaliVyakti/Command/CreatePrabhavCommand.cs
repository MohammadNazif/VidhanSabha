using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Command
{
    public class CreatePrabhavCommand : IRequest<int>
    {
        public CreatePrabhavshaliReqDto Dto { get; set; }
        public string UserId { get; set; }
        public CreatePrabhavCommand(CreatePrabhavshaliReqDto dto, string? userId)
        {
            Dto = dto;
            UserId = userId;
        }
    }
}
