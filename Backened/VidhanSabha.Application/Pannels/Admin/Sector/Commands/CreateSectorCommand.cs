using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Commands
{
    public class CreateSectorCommand : IRequest<int>
    {
        public CreateSectorRequestDto Dto { get; set; }
        public int CreatedById { get; set; }
        public string? CreatedBy { get; set; }
        public string CreatedByUserId { get; set; }

        public CreateSectorCommand(CreateSectorRequestDto dto, int createdById, string? createdBy ,string createdByUserId)
        {
            Dto = dto;
            CreatedById = createdById;
            CreatedBy = createdBy;
            CreatedByUserId = createdByUserId;
        }
    }
}
