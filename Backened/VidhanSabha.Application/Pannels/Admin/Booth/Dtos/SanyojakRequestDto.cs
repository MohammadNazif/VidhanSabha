using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Dtos
{
    public class SanyojakRequestDto
    {
        public string InchargeName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string FatherName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int CastId { get; set; }
        public string? EducationLevel { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        //public IFormFile? ProfileImage { get; set; }
    }

    public class SanyojakResponseDto
    {
        public string InchargeName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string FatherName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CastName { get; set; }
        public int CastId { get; set; }
        public string? EducationLevel { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
