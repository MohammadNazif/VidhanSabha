using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Command
{
    public class CreateSocialMediaCommandHandler : IRequestHandler<CreateSocialMediaCommand, int>
    {
        private readonly ISocialMediaRepository _repo;
        private readonly IImageService _imageService;
        public CreateSocialMediaCommandHandler(ISocialMediaRepository repo, IImageService imageService)
        {
            _repo = repo;
            _imageService = imageService;
        }
        public async Task<int> Handle(CreateSocialMediaCommand request, CancellationToken cancellationToken)
        {
            var imagePath = await request.Dto.ResolveImageAsync(
         _imageService,
         subFolder: "SocialMediaPost",
         imageSelector: dto => dto.PostImagePath
     );
            var req = request.Dto;

            var data = Tbl_SocialMediaPost.Create(req.UserId,
                req.Title,imagePath,req.Description,req.PlatformIds,req.BoothIds,req.SectorIds
                );

            return await _repo.AddAsync(data, cancellationToken);

        }
    }
}
