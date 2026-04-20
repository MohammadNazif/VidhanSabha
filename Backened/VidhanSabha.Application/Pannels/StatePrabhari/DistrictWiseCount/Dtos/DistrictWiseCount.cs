using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Dtos
{
    public class DistrictWiseCount
    {
        public sealed class VidhansabhaDistrictResponseDto
        {
            public int Id { get; init; }

            public int DistrictId { get; init; }
            public string DsitrictName { get; init; } = string.Empty;
            //public string VidhansabhaName { get; init; } = string.Empty;
            public int VidhanSabhaCount { get; init; }

            public int RemainingCount { get; set; }
          
        }

        public sealed class VidhansabhaDistrictRequestDto
        {

            public string UserId { get; set; } = string.Empty;
            public int StateId { get; set; }
            public int DistrictId { get; init; }
            public int VidhanSabhaCount { get; init; }
        }
    }
}

