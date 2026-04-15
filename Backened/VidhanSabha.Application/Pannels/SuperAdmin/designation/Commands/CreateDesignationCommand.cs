using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.DTOs;

namespace VidhanSabha.Application.Pannels.SuperAdmin.designation.Commands
{
    public class CreateDesignationCommand  : IRequest<int>
    {
        public  CreateDesignationDto Dto;
        public CreateDesignationCommand(CreateDesignationDto dto)
        {
            Dto = dto;
        }
    }
}
