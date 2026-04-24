using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.Command
{
    public class CreateBoothVoterCommand: IRequest<int>
    {
        public CreateBoothVoterRequestDto Dto { get; set; }
        public string UserId { get; set; }
        public CreateBoothVoterCommand(CreateBoothVoterRequestDto dto, string userId)
        {
            Dto = dto;
            UserId = userId;
        }
    }
}
