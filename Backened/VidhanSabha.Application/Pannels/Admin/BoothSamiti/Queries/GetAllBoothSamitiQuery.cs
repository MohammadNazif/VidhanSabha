using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Queries
{
    public class GetAllBoothSamitiQuery : IRequest<List<BoothSamitiResponseDto>>
    {
        public int Id { get; set; }
        public GetAllBoothSamitiQuery(int id)
        {
            Id = id;
        }
    }
}
