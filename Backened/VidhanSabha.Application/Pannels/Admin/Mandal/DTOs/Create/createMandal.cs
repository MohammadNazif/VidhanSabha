using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create
{
    public class MandalResponseDto
    {
        public int Id { get; set; }
        public int? VidhanId { get; set; }
        public string Name { get; set; }
        public bool IsMandalSanyojak { get; set; }
        public string? MandalSanyojak { get; set; }
        public string? Contact { get; set; }
        public int? CastId { get; set; }
        public int? CategoryId { get; set; }
        public string? CastName { get; set; }
        public string? FatherName { get; set; }
        public int? Age { get; set; }
        public string? Education { get; set; }
        public string? Address { get; set; }
        public string? Profile { get; set; }
        public bool Status { get; set; }
    }

    public class CreateMandalRequestDto 
    {
        public string? UserId { get; set; }
        public int? VidhanId { get; set; }
        public string Name { get; set; }
   
        public bool IsMandalSanyojak { get; init; }

        public SanyojakRequestDto? Sanyojak { get; init; }
    }

    public class UpdateMandalRequestDto : CreateMandalRequestDto
    {
        public int Id { get; set; }
    }
}
