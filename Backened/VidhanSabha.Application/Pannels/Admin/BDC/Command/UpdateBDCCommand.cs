using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Command
{
    public class UpdateBDCCommand : IRequest<int>
    {
        public UpdateBDCReqDto Dto;
        public UpdateBDCCommand(UpdateBDCReqDto dto)
        {
            Dto = dto;
        }
    }
}
