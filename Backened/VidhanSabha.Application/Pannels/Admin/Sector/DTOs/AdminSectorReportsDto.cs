using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Sector.DTOs
{
    public class AdminSectorReportsDto
    {
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public string SectorSanyojak { get; set; }
        public string Mobile { get; set; }
        public string Cast { get; set; }
        public List<VillageDto> Villages { get; set; }
        public int TotalBooth { get; set; }
        public int TotaVotes { get; set; }
        public int SeniorCitizen { get; set; }
        public int Handicap { get; set; }
        public int DoubleVoter { get; set; }
        public int PravasiVoter { get; set; }
    }

    
}
