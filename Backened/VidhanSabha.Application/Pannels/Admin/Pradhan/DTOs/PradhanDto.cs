using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs
{
        public class CreatePradhanRequestDto
        {
        public string Name { get; set; }
        public int DesignationId { get; set; }
        public string Contact { get; set; }
        public int Gender { get; set; }
        public List<int> VillageId { get; set; } = new();
        }
        public class PradhanResponseDto
        {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string Contact { get; set; }
        public int Gender { get; set; }
        public string GenderValue { get; set; }
        public List<VillageResponseDtos> Villages { get; set; } = new();
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
        public class UpdatePradhanRequestDto : CreatePradhanRequestDto
    {
            public int Id { get; set; }
        }
}
