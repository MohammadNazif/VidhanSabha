using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Command
{
    public class CreateSocialMediaCommandHandler : IRequestHandler<CreateSocialMediaCommand, int>
    {
        private readonly ISocialMediaRepository _repo;

        public CreateSocialMediaCommandHandler(ISocialMediaRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateSocialMediaCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;

            var data = Tbl_SocialMediaPost.Create(
                req.Title,req.PostImagePath,req.Description,req.PlatformIds,req.BoothIds,req.SectorIds
                );

            return await _repo.AddAsync(data, cancellationToken);

        }
    }
}
