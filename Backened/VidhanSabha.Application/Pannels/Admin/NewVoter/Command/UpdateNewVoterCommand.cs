using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Command
{
    public class UpdateNewVoterCommand:IRequest<int>
    {
        public UpdateNewVoterRequestDto Dto;
        public UpdateNewVoterCommand(UpdateNewVoterRequestDto dto)
        {
            Dto = dto;
        }

        
    }
}
