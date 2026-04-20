using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs
{
    public class CreateDoubleVoterReqDto
    {
        public int BoothId { get; set; }
        public List<int> VillageId { get; set; } = new();
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string VoterId { get; set; }
        public string PreviousAddress { get; set; }
        public string CurrentAddress { get; set; }
        public string Description { get; set; }
    }

    public class DoubleVoterResponseDto
    {
        public int Id { get; set; }
        public int BoothId { get; set; }
        public int BoothNumber { get; set; }
        public List<VillageResponseDtos> Villages { get; set; } = new();
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string VoterId { get; set; }
        public string PreviousAddress { get; set; }
        public string CurrentAddress { get; set; }
        public string Description { get; set; }
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
    public class UpdateDoubleVoterRequestDto : CreateDoubleVoterReqDto
    {
        public int Id { get; set; }
    }
}
