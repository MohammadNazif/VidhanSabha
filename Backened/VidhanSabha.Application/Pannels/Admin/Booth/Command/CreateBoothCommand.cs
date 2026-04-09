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
        public CreateBoothCommand(BoothRequestDto dto)
        {
            Dto = dto;
        }// null bhejenge jab IsBoothSanyojak = false
    }

}
