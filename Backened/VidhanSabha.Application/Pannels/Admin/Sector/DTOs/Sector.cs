using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Sector.DTOs
{
    public class CreateSectorRequestDto
    {
        public int MandalId { get; set; }
        public int VillageId { get; set; }
        public string SectorName { get; set; }
        public bool IsSectorSanyojak { get; set; }

        // Only required if IsSectorSanyojak = true
        public string? InchargeName { get; set; }
        public int? Age { get; set; }
        public string? FatherName { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }
        public string? EducationLevel { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ProfileImage { get; set; }
    }

    public class UpdateSectorRequestDto : CreateSectorRequestDto
    {
        public int Id { get; set; }
    }


    public class SectorByMAndalResponseDto
    {
        public int MandalId { get; set; }
        public int SectorId { get; set; }
        public string SectorName { get; set; }

    }
    public class SectorResponseDto
    {
        public int Id { get; set; }
        public int MandalId { get; set; }

        public string MandalName { get; set; }
        public string VillageName { get; set; }
        public string CategoryName { get; set; }
        public string CastName { get; set; }
        public int VillageId { get; set; }
        public string SectorName { get; set; }
        public bool IsSectorSanyojak { get; set; }
        public string? InchargeName { get; set; }
        public int? Age { get; set; }
        public string? FatherName { get; set; }
        public int? CategoryId { get; set; }
        public int? CastId { get; set; }
        public string? EducationLevel { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ProfileImage { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }


}
