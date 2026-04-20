using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Query
{
    public class getAllVidhanSabhaQuery : IRequest<IReadOnlyList<VidhanSabhaSatewiseResponseDto>>
    {
        public int? StateId { get; set; }

        public int? DistrictId { get; set; }
        public getAllVidhanSabhaQuery(int? stateId,int? districtId)
        {
            StateId = stateId;
            DistrictId = districtId;
        }
    }
}
