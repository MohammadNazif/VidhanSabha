using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Village.DTOs;

namespace VidhanSabha.Application.Common.Village.Queries
{
    public class GetallVillageByBoothId : IRequest<List<VillageByBoothResponseDto>>
    {
            public int id { get; set; }
    
            public GetallVillageByBoothId(int id)
            {
                this.id = id;
        }
    }
}
