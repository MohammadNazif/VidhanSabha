using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Influencer.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Influencer.Command
{
    public class DeleteInfluencerCommandHandler : IRequestHandler<DeleteInfluencerCommand, int>
    {
        private readonly IInfluencerRepository _repo;

        public DeleteInfluencerCommandHandler(IInfluencerRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(DeleteInfluencerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException("Influencer Not Found");
            }

            entity.Delete();

            return _repo.Update(entity);
        }
    }
}
