using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Block.Command
{
    public class CreateBlockCommand : IRequest<int>
    {
        public CreateBlockReqDto Dto { get; set; }
        public CreateBlockCommand(CreateBlockReqDto dto)
        {
            Dto = dto;
        }
    }
}
