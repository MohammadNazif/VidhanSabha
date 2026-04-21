using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Command
{
    public class CreateBDCCommand:IRequest<int>
    {
        public CreateBDCReqDto Dto;
        public CreateBDCCommand(CreateBDCReqDto dto)
        {
            Dto = dto;
        }
    }
}
