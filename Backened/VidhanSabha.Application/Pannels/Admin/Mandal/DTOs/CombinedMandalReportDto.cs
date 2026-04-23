using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.DTOs
{
    public class CombinedMandalReportDto
    {
        public int MandalId { get; set; }
        public string MandalName { get; set; }
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public string SectorSanyojak { get; set; }
        public string SectorSanyojakContact { get; set; }
        public int BoothId { get; set; }
        public int BoothNumber { get; set; }
        public string PolllingStationName { get; set; }
        public string BoothAdhyaksh { get; set; }
        public string BoothAdhyakshContact { get; set; }
        public int BoothAdhyakshVillageId { get; set; }
        public string BoothAdhyakshVillageName { get; set; }
        public string BoothAdhyakshFather { get; set; }
        public int BoothAdhyakshAge { get; set; }
        public int BoothAdhyakshCastId { get; set; }
        public string BoothAdhyakshCastName { get; set; }
        public string BoothAdhyakshAddress { get; set; }
        public string BoothAdhyakshEducation { get; set; }
        public string ProfileImagePath { get; set; }
    }
}
