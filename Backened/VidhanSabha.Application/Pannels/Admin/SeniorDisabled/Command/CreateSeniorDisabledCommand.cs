using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Command
{
    public class CreateSeniorDisabledCommand : IRequest<int>
    {
        public CreateSeniorDisabledReqDto Dto { get; set; }
        public string UserId { get; set; }
        public CreateSeniorDisabledCommand(CreateSeniorDisabledReqDto dto, string? userId)
        {
            Dto = dto;
            UserId = userId;
        }
    }
}
