using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    internal class GetSectorByMandalIdQuery : IRequest<SectorResponseDto>
    {
        public int Id { get; set; }
    }
}
