
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace VidhanSabha.Application.Pannels.Admin.Sector.DTOs
{
    public class CreateSectorRequestDto
    {
        public int MandalId { get; set; }
        public List<int> VillageIds { get; set; }  

      //  [JsonIgnore]
      //  public List<int> VillageId => VillageIds?
      //  .Split(',', StringSplitOptions.RemoveEmptyEntries)
      //  .Select(x => {
      //  if (int.TryParse(x.Trim(), out var id))
      //      return (int?)id;
      //  return null;
      // })
      //.Where(x => x.HasValue)
      //.Select(x => x.Value)
      //.ToList() ?? new List<int>();
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
        public IFormFile? ProfileImage { get; init; }
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
        //public string VillageName { get; set; }
        public string CategoryName { get; set; }
        public string CastName { get; set; }
        public List<VillageResponseDto>? Villages { get; set; }
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
        public string? Profile { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class VillageResponseDto
    {
        public int VillageId { get; set; }
        public string VillageName { get; set; }
    }



}
