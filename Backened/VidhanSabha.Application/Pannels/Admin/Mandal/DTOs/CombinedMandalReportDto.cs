using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.DTOs
{
    public class MandalFullDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public List<SectorDto>? Sectors { get; set; }
    }

    public class SectorDto
    {
        public int Id { get; set; }
        public string SectorName { get; set; }
        public string InchargeName { get; set; }
        public string PhoneNumber { get; set; }

        public BoothDto Booth { get; set; }
    }

    public class BoothDto
    {
        public int Id { get; set; }
        public int BoothNumber { get; set; }
        public string PollingStationName { get; set; }

        public SanyojakDto Sanyojak { get; set; }
        public List<VillageDto>? Villages { get; set; }
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
