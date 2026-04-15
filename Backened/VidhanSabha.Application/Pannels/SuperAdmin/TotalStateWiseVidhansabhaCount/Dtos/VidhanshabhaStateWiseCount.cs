using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Dtos
{
    public class VidhanshabhaStateWiseCount
    {
        public sealed class VidhansabhaResponseDto
        {
            public int Id { get; init; }
            public int StateId { get; init; }
            public string StateName { get; init; } = string.Empty;
            //public string VidhansabhaName { get; init; } = string.Empty;
            public int VidhanSabhaCount { get; init; }

            public int RemainingCount { get; set; }
            //public bool Status { get; init; }
        }

        public sealed class VidhansabhaRequestDto
        {
          
            public int StateId { get; init; }
            public int VidhanSabhaCount { get; init; }
        }
    }
}
