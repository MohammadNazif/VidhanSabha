using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Category.DTOs;
using VidhanSabha.Application.Common.Village.DTOs;

namespace VidhanSabha.Application.Common.Category.Queries
{
    public class GetallVillage : IRequest<List<VillageResponseDto>>
    {
        public int id { get; set; }

        public GetallVillage(int id)
        {
            this.id = id;
        }
    }
}
