using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos
{
    public class CreateVidhanSabhaRequestDto
    {
        public string UserId { get;  set; }
        public string VidhanSabhaName { get;  set; }

        public int VidhanSabhaId { get; set; }
        public int vidhanSabhaNumber { get;  set; }
        public int DistrictId { get;  set; }

        public int stateId { get; set; }
        public bool isPrabhari { get; set; }
        public CreatePrabhariRequestDto? Prabhari { get; set; }
    }

    public class UpdateVidhanSabhaRequestDto
    {
        public int Id { get; set; }
        public string VidhanSabhaName { get; set; }
        public int VidhanSabhaNumber { get; set; }

    }

    public class VidhanSabhaSatewiseResponseDto
    {
        public int Id { get; set; }
        public string VidhanSabhaName { get; set; }
        public int VidhanSabhaNumber { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }

        public int vidhanSabhaId { get; set; }
        public bool? HasPrabhari { get; set; }

    }
}
