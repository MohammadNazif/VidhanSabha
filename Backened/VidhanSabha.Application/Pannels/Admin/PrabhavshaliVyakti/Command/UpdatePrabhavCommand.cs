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
    public class UpdatePrabhavCommand : IRequest<int>
    {
        public UpdatePrabhavshaliReqDto Dto;
        public UpdatePrabhavCommand(UpdatePrabhavshaliReqDto dto)
        {
            Dto = dto;
        }
    }
}
