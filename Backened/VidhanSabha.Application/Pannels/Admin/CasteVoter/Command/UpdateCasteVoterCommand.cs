using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.CasteVoter.Command
{
    public class UpdateCasteVoterCommand : IRequest<int>
    {
        public UpdateCasteVoterReqDto Dto;
        public UpdateCasteVoterCommand(UpdateCasteVoterReqDto dto)
        {
            Dto = dto;
        }

    }
}
