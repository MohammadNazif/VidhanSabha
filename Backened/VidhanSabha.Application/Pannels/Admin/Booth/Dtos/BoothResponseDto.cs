using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Dtos
{
    public class BoothResponseDto
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public int MandalId { get; set; }

        public string MandalName { get; set; }
        public int SectorId { get; set; }

        public string SectorName { get; set; }
        public int BoothNumber { get; set; }
        public string PollingStationName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PollingStationLocation { get; set; } = string.Empty;
        public bool IsBoothSanyojak { get; set; }

        public string VillageNames { get; set; }
        public List<VillageResponseDto> Villages { get; set; } = new();
        public SanyojakResponseDto? Sanyojak { get; set; }   
    }

    public class BoothRequestDto
    {
        public int MandalId { get; init; }
        public int SectorId { get; init; }
        public List<VillageInputDto> Villages { get; init; } = new();
        public int BoothNumber { get; init; }
        public string PollingStationName { get; init; } = string.Empty;
        public string PollingStationLocation { get; init; } = string.Empty;
        public bool IsBoothSanyojak { get; init; }

        public SanyojakRequestDto? Sanyojak { get; init; }
    }

    public class updateBoothRequestDto  : BoothRequestDto
    {
        public int id { get; set; }
    }
}
