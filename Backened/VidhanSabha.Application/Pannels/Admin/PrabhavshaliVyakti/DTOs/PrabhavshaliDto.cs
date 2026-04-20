using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs
{
    public class CreatePrabhavshaliReqDto
    {
        public int BoothId { get; set; }
        public int DesignationId { get; set; }
        public List<int> VillageId { get; set; } = new();
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int CastId { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
    }
    public class PrabhavshaliResponseDto
    {        
        public int Id { get; set; }
        public int BoothId { get; set; }
        public int BoothNumber { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public List<VillageResponseDtos> Villages { get; set; } = new();
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CastId { get; set; }
        public string CastName { get; set; }
        public string Mobile { get; set; }
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
    public class UpdatePrabhavshaliReqDto : CreatePrabhavshaliReqDto
    {
        public int Id { get; set; }
    }
}
