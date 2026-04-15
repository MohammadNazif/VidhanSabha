using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos
{
    public class CreatePannaPramukhRequestDto
    {
        public int BoothId { get; set; }

        public List<int> VillageId { get; set; } = new();

        public string PannaPramukhName { get; set; } = string.Empty;
        public int PannaNumber { get; set; }
        public int CategoryId { get; set; }
        public int CastId { get; set; }
        public string VoterId { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        //public IFormFile? ProfilePicture { get; set; }

        
    }
    public class PannaPramukhResponseDto
    {
        public int Id { get; set; }
        public int BoothId { get; set; }
        public int BoothNumber { get; set; }
        public string PollingStationName { get; set; } = string.Empty;
        public int PannaNumber { get; set; }
        public string PannaPramukhName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CastId { get; set; }
        public string CastName { get; set; } = string.Empty;
        public string VoterId { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public List<VillageDto> Villages { get; set; } = new();
    }

    public class VillageDto
    {
        public int VillageId { get; set; }
        public string VillageName { get; set; } = string.Empty;
    }
    // ── Update Request ────────────────────────────────────────────
    public class UpdatePannaPramukhRequestDto : CreatePannaPramukhRequestDto
    {
        public int Id { get; set; }
 
    }
}
