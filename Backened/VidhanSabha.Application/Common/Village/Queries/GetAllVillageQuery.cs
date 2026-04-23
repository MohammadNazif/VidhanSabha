using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Village.DTOs;

namespace VidhanSabha.Application.Common.Village.Queries
{
    public class GetAllVillageQuery:IRequest<List<VillageResponseDtos>>
    {
    }
}
