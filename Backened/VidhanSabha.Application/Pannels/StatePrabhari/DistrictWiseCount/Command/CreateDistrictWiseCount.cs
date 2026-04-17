using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using static VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Dtos.DistrictWiseCount;
using static VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Dtos.VidhanshabhaStateWiseCount;

namespace VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Command
{
    public class CreateDistrictWiseCount : IRequest<int>
    {
        public VidhansabhaDistrictRequestDto Dto { get; set; }
        public CreateDistrictWiseCount(VidhansabhaDistrictRequestDto dto)
        {
            Dto = dto;
        }
    }
}
