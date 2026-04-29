using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Queries
{
    public class GetBoothByIdQuery : IRequest<BoothSamitiMemResponseDto>
    {
        public int BoothId { get; set; }

        public GetBoothByIdQuery(int boothId)
        {
            BoothId = boothId;
        }
    }
}
