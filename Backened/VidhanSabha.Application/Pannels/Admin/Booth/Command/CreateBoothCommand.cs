using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Command
{
    public record CreateBoothCommand : IRequest<int>
    {
        public BoothRequestDto Dto;
        public string UserId;

        public CreateBoothCommand(BoothRequestDto dto,string UserId)
        {
            Dto = dto;
            UserId = UserId;
        }// null bhejenge jab IsBoothSanyojak = false
    }

}
