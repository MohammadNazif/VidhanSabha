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
    public class UpdateBoothVoterCommand : IRequest<int>
    {
        public UpdateBoothVoterRequestDto Dto;
        public UpdateBoothVoterCommand(UpdateBoothVoterRequestDto dto)
        {
            Dto = dto;
        }
    }
}
