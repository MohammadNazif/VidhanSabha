using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using static VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Dtos.DistrictWiseCount;

namespace VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Command
{
    public class UpdateDistrictWiseCount : IRequest<int>
    {

        public UpdateVidhansabhaDistrictReqDto Dto;
        public UpdateDistrictWiseCount(UpdateVidhansabhaDistrictReqDto dto)
        {
            Dto = dto;
        }
    }
}
