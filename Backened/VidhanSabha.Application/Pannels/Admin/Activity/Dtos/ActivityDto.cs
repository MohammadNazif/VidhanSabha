using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Activity.Dtos
{
    public class CreateActivityDto
    {
        public string? UserId { get; set; }
        public string Title { get; set; } = default!;
        public DateTime Date { get; set; }
        public string Description { get; set; } = default!;

        // Provide ONE of the two below
        public string? YouTubeLink { get; set; }
        public IFormFile? VideoFile { get; set; }

        // Up to 4 images
        public List<IFormFile>? Images { get; set; }
    }

    public class ActivityResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public DateTime Date { get; set; }
        public string Description { get; set; } = default!;
        public string? YouTubeLink { get; set; }
        public string? VideoPath { get; set; }
        public List<string> ImagePaths { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ActivityQueryParams : BaseQueryParams
    {
        public int? Id { get; set; }

    }
}
