using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Command
{
    public class CreateNewVoterCommand : IRequest<int>
    {
        public CreateNewVoterRequestDto Dto { get; set; }
        public string UserId { get; set; }
        public CreateNewVoterCommand(CreateNewVoterRequestDto dto, string? userId)
        {
            Dto = dto;
            UserId = userId;
        }
    }
}
