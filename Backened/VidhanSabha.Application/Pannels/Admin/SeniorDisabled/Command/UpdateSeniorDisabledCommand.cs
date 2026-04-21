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
    public class UpdateSeniorDisabledCommand : IRequest<int>
    {
        public UpdateSeniorDisabledReqDto Dto;
        public UpdateSeniorDisabledCommand(UpdateSeniorDisabledReqDto dto)
        {
            Dto = dto;
        }

    }
}
