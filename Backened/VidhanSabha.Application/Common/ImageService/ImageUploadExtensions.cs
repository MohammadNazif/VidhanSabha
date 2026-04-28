using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Exceptions;

namespace VidhanSabha.Application.Common.ImageService
{
    public static class ImageUploadExtensions
    {
        /// <summary>
        /// Call this inside any command handler to extract, validate,
        /// and save an image from the DTO before hitting the DB.
        /// </summary>
        public static async Task<string?> ResolveImageAsync<TDto>(
            this TDto dto,
            IImageService imageService,
            string subFolder,
            Func<TDto, IFormFile?> imageSelector)
        {
            var file = imageSelector(dto);

            if (file == null) return null;

            //if (!imageService.IsValidImage(file))
            //    throw new ValidationException();

            return await imageService.SaveImageAsync(file, subFolder);
        }
    }
}
