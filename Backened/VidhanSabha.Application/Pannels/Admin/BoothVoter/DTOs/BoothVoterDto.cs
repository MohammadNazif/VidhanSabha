using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs
{
    public class CreateBoothVoterRequestDto
    {
        public int BoothId { get; set; }
        public List<int> VillageId { get; set; } = new();
        public int TotalVoter { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Other { get; set; }
    }
    public class BoothVoterResponseDto
    {
        public int Id { get; set; }
        public int BoothId { get; set; }
        public int BoothNumber { get; set; }
        public string PollingStation { get; set; } = string.Empty;
        public List<BoothVoterVillageResponseDtos> Villages { get; set; } = new();
        public int Male { get; set; }
        public int Female { get; set; }
        public int Other { get; set; }
        public int TotalVoter { get; set; }
    }
    public class BoothVoterVillageResponseDtos
    {
        public int VillageId { get; set; }
        public string VillageName { get; set; } = string.Empty;
    }

    public class BoothVoterVillageReqDto
    {
        public int VillageId { get; set; }

    }
    public class UpdateBoothVoterRequestDto : CreateBoothVoterRequestDto
    {
        public int Id { get; set; }
    }
}
