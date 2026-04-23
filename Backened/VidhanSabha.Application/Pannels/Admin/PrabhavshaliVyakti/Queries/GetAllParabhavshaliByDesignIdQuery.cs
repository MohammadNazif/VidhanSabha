using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Queries
{
    public class GetAllParabhavshaliByDesignIdQuery:IRequest<List<PrabhavshaliResponseDesinIdDto>>
    {
        public int DesgId { get; set; }
    }
}
