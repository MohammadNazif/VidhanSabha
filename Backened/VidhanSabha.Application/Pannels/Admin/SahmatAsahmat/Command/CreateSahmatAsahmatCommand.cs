using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Command
{
    public class CreateSahmatAsahmatCommand : IRequest<int>
    {
        public CreateSahmatAsahmatReqDto Dto { get; set; }
        public string UserId { get; set; }

        public string Role { get; set; }
        public CreateSahmatAsahmatCommand(CreateSahmatAsahmatReqDto dto, string? userId,string role)
        {
            Dto = dto;
            UserId = userId;
            Role = role;
        }
    }
}
