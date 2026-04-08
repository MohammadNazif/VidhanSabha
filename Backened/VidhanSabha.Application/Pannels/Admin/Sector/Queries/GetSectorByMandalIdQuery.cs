using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    public class GetSectorByMandalIdQuery : IRequest<List<SectorByMAndalResponseDto>>
    {
        public int Id { get; set; }
        public GetSectorByMandalIdQuery(int id)
        {
            Id = id;
        }
    }
}
