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
    public class DeleteSocialMediaCommandHandler : IRequestHandler<DeleteSocialMediaCommand, int>
    {
        private readonly ISocialMediaRepository _repo;

        public DeleteSocialMediaCommandHandler(ISocialMediaRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteSocialMediaCommand request, CancellationToken cancellationtoken)
        {
            
            var social = await _repo.GetByIdAsync(request.Id);
            if (social == null)
            {
                throw new NotFoundException("Social Media Post Not Found");
            }
            social.Delete();
            return _repo.Update(social);
        }
    }
}
