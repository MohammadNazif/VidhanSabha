using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Command
{
    public class updateBoothCommand : IRequest<bool>
    {
         public updateBoothRequestDto Dto;
       
        public updateBoothCommand(updateBoothRequestDto dto)
        {
            Dto = dto; 
        }
    }
}
