using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    public class getSectorInchargeQuery : IRequest<IReadOnlyList<SectorIncahrgeDto>> 
    {
        public string UserId { get; set; }
        public getSectorInchargeQuery(string userId)
        {
            UserId = userId;
        }
    }
}
