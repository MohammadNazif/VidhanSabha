using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Commands
{
    public class UpdateSectorCommand : IRequest<int>
    {
        public UpdateSectorRequestDto Dto { get; set; }

        public UpdateSectorCommand(UpdateSectorRequestDto dto)
        {
            Dto = dto;
        }
    }
}
