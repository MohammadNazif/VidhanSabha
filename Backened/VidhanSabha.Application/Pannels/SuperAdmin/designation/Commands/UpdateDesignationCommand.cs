using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.DTOs;

namespace VidhanSabha.Application.Pannels.SuperAdmin.designation.Commands
{
    public class UpdateDesignationCommand : IRequest<int>
    {
        public UpdateDesignationDto Dto;
        public UpdateDesignationCommand(UpdateDesignationDto dto)
        {
            Dto = dto;
        }
    }
}
