using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.DTOs
{
    public class CreateSocialMediaPostReqDto
    {
        public string Title { get; set; }
        public IFormFile? PostImagePath { get; set; }
        public string Description { get; set; }
        public List<int> PlatformIds { get; set; } = new();
        public List<int> BoothIds { get; set; } = new();
        public List<int> SectorIds { get; set; } = new();
    }
    public class UpdateSocialMediaPostReq:CreateSocialMediaPostReqDto
    {
        public int Id { get; set; }
    }
    public class SocialMediaPostReponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? PostImagePath { get; set; }
        public string Description { get; set; }
        public List<SocialMediaPlatformResponseDto> Platforms { get; set; } = new();
        public List<SocialMediaBoothResponseDto> Booths { get; set; } = new();
        public List<SocialMediaSectorResponseDto> Sectors { get; set; } = new();

    }

    public class SocialMediaPlatformResponseDto
    {
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
    }
    public class SocialMediaBoothResponseDto
    {
        public int BoothId { get; set; }
        public int BoothNo { get; set; }
    }

    public class SocialMediaSectorResponseDto
    {
        public int SectorId { get; set; }
        public string SectorName { get; set; }
    }

    public class SocialMediaPlatform
    {
        public int Id { get; set; }
        public string PlatformName{ get; set; }
    }
}
