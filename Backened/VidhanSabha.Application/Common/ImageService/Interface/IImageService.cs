using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VidhanSabha.Application.Common.ImageService.Interface
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile? file, string subFolder);
        Task<string?> UpdateImageAsync(IFormFile? file, string? oldRelativePath, string subFolder);
        Task DeleteImageAsync(string? relativePath);
        bool IsValidImage(IFormFile? file);
    }
}
