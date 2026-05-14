using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.DTOs
{
    public class MandalFullDto
    {
        // 🔹 Mandal
        public int MandalId { get; set; }
        public string? MandalName { get; set; }

        // 🔹 Sector
        public int SectorId { get; set; }
        public string? SectorName { get; set; }
        public string? SectorInchargeName { get; set; }
        public string? SectorFatherName { get; set; }
        public string? SectorPhone { get; set; }

        // 🔹 Booth
        public int? BoothId { get; set; }
        public int? BoothNumber { get; set; }
        public string? PollingStationName { get; set; }

        // 🔹 Sanyojak
        public string? SanyojakName { get; set; }
        public string? SanyojakPhone { get; set; }
        public string? SanyojakFatherName { get; set; }
        public int? SanyojakAge { get; set; }
        public string? SanyojakCaste { get; set; }
        public string? SanyojakAddress { get; set; }
        public string? SanyojakEducation { get; set; }
        public string? SanyojakProfile { get; set; }

        // 🔹 Village
        public int? VillageId { get; set; }
        public string? VillageNames { get; set; }
    }

    //public class SectorDto
    //{
    //    public int Id { get; set; }
    //    public string SectorName { get; set; }
    //    public string InchargeName { get; set; }

    //    public string FatherName { get; set; }
    //    public string PhoneNumber { get; set; }

    //    public BoothDto Booth { get; set; }
    //}

    //public class BoothDto
    //{
    //    public int Id { get; set; }
    //    public int BoothNumber { get; set; }
    //    public string PollingStationName { get; set; }

    //    public SanyojakDto Sanyojak { get; set; }
    //    public List<VillageDto>? Villages { get; set; }
    //}

    //public class SanyojakDto
    //{
    //    public string Name { get; set; }
    //    public string Phone { get; set; }
    //    public string FatherName { get; set; }
    //    public int Age { get; set; }
    //    public string CastName { get; set; }
    //    public string Address { get; set; }
    //    public string Education { get; set; }
    //    public string ProfilePath { get; set; }
    //}

    //public class VillageDto
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}
