using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.BDC.DTOs
{
    public class CreateBDCReqDto
    {
        public string Block { get; set; }
        public string Name { get; set; }
        public string WardNumber { get; set; }
        public List<int> VillageId { get; set; } = new();
        public int CategoryId { get; set; }
        public int CastId { get; set; }
        public int Age { get; set; }
        public string Mobile { get; set; }
        public int PartyId { get; set; }
        public string Education { get; set; }
        public IFormFile? Profile { get; init; }
    }
    public class BDCResponseDto
    {
        public int Id { get; set; }
        public string Block { get; set; }
        public string Name { get; set; }
        public string WardNumber { get; set; }
        public List<VillageResponseDto> Villages { get; set; } = new();
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CastId { get; set; }
        public string CastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Mobile { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string Education { get; set; }

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
    public class UpdateBDCReqDto : CreateBDCReqDto
    {
        public int Id { get; set; }
    }
}
