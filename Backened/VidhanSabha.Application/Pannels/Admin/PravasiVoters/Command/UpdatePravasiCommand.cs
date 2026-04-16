using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command
{
    public class UpdatePravasiCommand:IRequest<int>
    {
        public UpdatePravasiVoterRequestDto Dto;
        public UpdatePravasiCommand(UpdatePravasiVoterRequestDto dto)
        {
            Dto = dto;
        }
    }
}
