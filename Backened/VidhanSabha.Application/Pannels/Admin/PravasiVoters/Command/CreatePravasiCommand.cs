using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command
{
    public class CreatePravasiCommand : IRequest<int>
    {
        public CreatePravasiVoterRequestDto Dto { get; set; }
        public string UserId { get; set; }
        public CreatePravasiCommand(CreatePravasiVoterRequestDto dto,string? userId)
        {
            Dto = dto;
            UserId = userId;
        }
    }
}
