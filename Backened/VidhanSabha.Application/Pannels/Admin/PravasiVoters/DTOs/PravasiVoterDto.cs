using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs
{
    public class CreatePravasiVoterRequestDto
    {
        public int BoothId { get; set; }
        public List<int> VillageId { get; set; } = new();
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int CategoryId { get; set; }
        public int CastId { get; set; }
        public int OccupationId { get; set; }
        public string VoterId { get; set; }
        public string CurrentAddress { get; set; }
    }
    public class PravasiVoterResponseDto
    {
        public int Id { get; set; }
        public int BoothId { get; set; }
        public int BoothNumber { get; set; }
        public List<VillageResponseDto> Villages { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public string Mobile { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CastId { get; set; }
        public string CastName { get; set; } = string.Empty;
        public int OccupationId { get; set; }
        public string Occupation { get; set; }
        public string VoterId { get; set; }
        public string CurrentAddress { get; set; }
        
    }
    public class VillageResponseDto
    {
        public int VillageId { get; set; }
        public string VillageName { get; set; } = string.Empty;
    }

    public class VillageReqDto
    {
        public int VillageId { get; set; }
      
    }
    public class UpdatePravasiVoterRequestDto:CreatePravasiVoterRequestDto
    {
        public int Id { get; set; }
    }
}
