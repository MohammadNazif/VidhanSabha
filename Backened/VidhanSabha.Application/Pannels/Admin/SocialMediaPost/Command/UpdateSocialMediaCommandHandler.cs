using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Command
{
    public class UpdateSocialMediaCommandHandler : IRequestHandler<UpdateSocialMediaCommand, int>
    {
        private readonly ISocialMediaRepository _repo;

        public UpdateSocialMediaCommandHandler(ISocialMediaRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(UpdateSocialMediaCommand request, CancellationToken cancellationtoken)
        {
            var req = request.Dto;
            var social = await _repo.GetByIdAsync(req.Id);
            if (social == null)
            {
                throw new NotFoundException("Social Media Post Not Found");
            }
            social.Update(
                req.Title, req.PostImagePath, req.Description, req.PlatformIds, req.BoothIds, req.SectorIds
                );
            return _repo.Update(social);
        }
    }
}
