using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs
{
    
        public class CreateNewVoterRequestDto
        {
            public int BoothId { get; set; }
            public List<int> VillageId { get; set; } = new();
            public string Name { get; set; }
            public string FatherName { get; set; }
            public string Mobile { get; set; }
            public int CategoryId { get; set; }
            public int CastId { get; set; }
            public DateOnly DOB { get; set; }
            public int Age { get; set; }
            public string VoterId { get; set; }
        }
        public class NewVoterResponseDto
        {
            public int Id { get; set; }
            public int BoothId { get; set; }
            public int BoothNumber { get; set; }
            public List<VillageResponseDtos> Villages { get; set; } = new();
            public string Name { get; set; } = string.Empty;
            public string FatherName { get; set; }
            public string Mobile { get; set; }
            public int CategoryId { get; set; }
            public string CategoryName { get; set; } = string.Empty;
            public int CastId { get; set; }
            public string CastName { get; set; } = string.Empty;
            public DateOnly DOB { get; set; }
            public int Age { get; set; }
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
        public class UpdateNewVoterRequestDto : CreateNewVoterRequestDto
        {
            public int Id { get; set; }
        }
}
