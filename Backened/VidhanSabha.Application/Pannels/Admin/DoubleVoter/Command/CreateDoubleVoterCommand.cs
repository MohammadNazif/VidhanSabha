using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.DoubleVoter.Command
{
    public class CreateDoubleVoterCommand : IRequest<int>
    {
        public CreateDoubleVoterReqDto Dto { get; set; }
        public CreateDoubleVoterCommand(CreateDoubleVoterReqDto dto)
        {
            Dto = dto;
        }
    }
}
