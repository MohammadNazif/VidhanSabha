using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Activity.Dtos;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Activity.Command
{
    public class updateActivityCommand:IRequest<int>
    {
        public UpdateActivityDto Dto;
        public updateActivityCommand(UpdateActivityDto dto)
        {
            Dto = dto;
        }
    }
}
