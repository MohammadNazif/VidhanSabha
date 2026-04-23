using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs
{
    public class SeniorDisabledReq
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int CategoryId { get; set; }
        public int CastId { get; set; }
        public string Mobile { get; set; }
        public string VoterId { get; set; }
    }
    public class CreateSeniorDisabledReqDto
    {
        public int TypeId { get; set; }
        public int BoothId { get; set; }
        public List<int> VillageId { get; set; } = new();
        public List<SeniorDisabledReq> SeniorDisabledRequest { get; set; } = new();
    }
    public class SeniorDisabledResponseDto
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public string SectorSanyojak { get; set; }
        public int BoothId { get; set; }
        public int BoothNumber { get; set; }
        public List<VillageResponseDtos> Villages { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CastId { get; set; }
        public string CastName { get; set; } = string.Empty;
        public string Mobile { get; set; }
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
    public class UpdateSeniorDisabledReqDto 
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int BoothId { get; set; }
        public List<int> VillageId { get; set; } = new();
        public string Name { get; set; }
        public string Address { get; set; }
        public int CategoryId { get; set; }
        public int CastId { get; set; }
        public string Mobile { get; set; }
        public string VoterId { get; set; }
    }
}
