using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using static VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Dtos.DistrictWiseCount;

namespace VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Query
{
    public class getAllDitrictWiseCountQuery : IRequest<IReadOnlyList<VidhansabhaDistrictResponseDto>>
    {
        public string UserId { get; set; }
        public getAllDitrictWiseCountQuery(string userId)
        {
            UserId = userId;
        }
    }
}
