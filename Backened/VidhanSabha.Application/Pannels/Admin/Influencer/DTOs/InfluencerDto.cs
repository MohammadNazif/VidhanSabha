using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Village.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Influencer.DTOs
{
    public class CreateInfluencerReqDto
    {
        public int BoothId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int CastId { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
        public List<int> VillageIds { get; set; } = new();
    }

    public class UpdateInfluencerReqDto : CreateInfluencerReqDto
    {
        public int Id { get; set; }
    }

    public class InfluencerResponseDto
    {
        public int Id { get; set; }
        public int BoothId { get; set; }
        public int BoothNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CastId { get; set; }
        public string CastName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<InfluencerVillageResponseDtos> Villages { get; set; } = new();
    }

    public class InfluencerVillageResponseDtos
    {
        public int VillageIds { get; set; }
        public string VillageName { get; set; } = string.Empty;
    }
    public class InfluencerVillageReqDtos
    {
        public int VillageId { get; set; }
    }
}