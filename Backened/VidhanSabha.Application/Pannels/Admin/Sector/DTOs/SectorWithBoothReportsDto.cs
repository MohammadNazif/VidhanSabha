using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Sector.DTOs
{
    public class SectorReportDto
    {
        // 🔹 Mandal
        public int MandalId { get; set; }
        public string MandalName { get; set; }

        // 🔹 Sector
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public string InchargeName { get; set; }
        public int? Age { get; set; }
        public string FatherName { get; set; }
        public int? CastId { get; set; }
        public string CastName { get; set; }
        public string EducationLevel { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ProfileImage { get; set; }

        // 🔥 NEW → Sector Villages
        public List<VillageDto> SectorVillages { get; set; }

        // 🔹 Booth
        public BoothDto Booth { get; set; }
    }

    public class BoothDto
    {
        public int Id { get; set; }
        public int BoothNumber { get; set; }
        public string PollingStationName { get; set; }

        public SanyojakDto Sanyojak { get; set; }
        public List<VillageDto> Villages { get; set; }
    }

    public class SanyojakDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string FatherName { get; set; }
        public int Age { get; set; }
        public string CastName { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
        public string ProfilePath { get; set; }
    }

    public class VillageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
