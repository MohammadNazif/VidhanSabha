using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Command
{
    public class UpdateSahmatAsahmatCommand:IRequest<int>
    {
        public UpdateSahmatAsahmatReqDto Dto;
        public UpdateSahmatAsahmatCommand(UpdateSahmatAsahmatReqDto dto)
        {
            Dto = dto;
        }
    }
}
