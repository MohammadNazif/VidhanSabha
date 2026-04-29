using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Dtos
{
    public class BoothReportsDto
    {
        public int BoothId { get; set; }
        public int BoothNo { get; set; }
        public string PollingStation { get; set; }
        public string BoothAdhyaksh { get; set; }
        public string Mobile { get; set; }
        public List<VillageResponseDto> Villages { get; set; }
        public string Cast { get; set; }
        public int TotalVotes { get; set; }
        public int SeniorCitizen { get; set; }
        public int Handicap { get; set; }
        public int DoubleVotes { get; set; }
        public int Pravasi { get; set; }
    }
}
