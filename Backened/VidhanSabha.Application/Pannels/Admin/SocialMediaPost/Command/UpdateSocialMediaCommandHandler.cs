using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Command
{
    public class UpdateSocialMediaCommandHandler : IRequestHandler<UpdateSocialMediaCommand, int>
    {
        private readonly ISocialMediaRepository _repo;
        private readonly IImageService _imageService;
        public UpdateSocialMediaCommandHandler(ISocialMediaRepository repo, IImageService imageService)
        {
            _repo = repo;
            _imageService = imageService;
        }

        public async Task<int> Handle(UpdateSocialMediaCommand request, CancellationToken cancellationtoken)
        {
            var req = request.Dto;
            var social = await _repo.GetByIdAsync(req.Id);
            
            if (social == null)
            {
                throw new NotFoundException("Social Media Post Not Found");
            }

            string? newImagePath = null;
            if (req.PostImagePath != null)
            {
                //if (!_imageService.IsValidImage(request.ProfileImage))
                //    throw new ValidationException("Invalid image. Only JPG/PNG/WEBP under 5MB allowed.");

                newImagePath = await _imageService.SaveImageAsync(
                    req.PostImagePath,
                    subFolder: "SocialMediaPost"
                );

                // Delete old image only after new one saved successfully
                await _imageService.DeleteImageAsync(social.PostImagePath);
            }
            
            social.Update(
                req.Title,newImagePath, req.Description, req.PlatformIds, req.BoothIds, req.SectorIds
                );
            return _repo.Update(social);
        }
    }
}
