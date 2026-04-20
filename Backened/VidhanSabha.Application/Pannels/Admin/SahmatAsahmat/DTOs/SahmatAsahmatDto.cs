using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs
{
    public class CreateSahmatAsahmatReqDto
    {
        public int BoothId { get; set; }
        public int TypeId { get;  set; }
        public List<int> VillageId { get; set; } = new();
        //public bool IsAsahmat { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Mobile { get; set; }
        public int PartyId { get; set; }
        public int OccupationId { get; set; }
        public string Reason { get; set; }
        public string VoterId { get; set; }
    }
    public class SahmatAsahmatResponseDto
    {
        public int Id { get; set; }
        public int BoothId { get; set; }
        public int BoothNumber { get; set;}
        public int TypeId { get; set; }
        public string Type { get; set; }
        public List<VillageResponseDtos> Villages { get; set; } = new();
        //public bool IsAsahmat { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Mobile { get; set; }
        public int PartyId { get; set; }
        public string Party { get; set; }
        public int OccupationId { get; set; }
        public string Occupation { get; set; }
        public string Reason { get; set; }
        public string VoterId { get; set; }
    }
    public class VillageResponseDtos
    {
        public int VillageId { get; set; }
        public string VillageName { get; set; } = string.Empty;
    }

    public class VillageReqDto
    {
        public int VillageId { get; set; }

    }
    public class UpdateSahmatAsahmatReqDto : CreateSahmatAsahmatReqDto
    {
        public int Id { get; set; }
    }
}
