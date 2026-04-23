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
    namespace VidhanSabha.Application.Pannels.Admin.Influencer.Command
    {
        public class UpdateInfluencerCommandHandler : IRequestHandler<UpdateInfluencerCommand, int>
        {
            private readonly IInfluencerRepository _repo;

            public UpdateInfluencerCommandHandler(IInfluencerRepository repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(UpdateInfluencerCommand request, CancellationToken cancellationToken)
            {
                var dto = request.Dto;
                var entity = await _repo.GetByIdAsync(dto.Id, cancellationToken);

                if (entity == null)
                {
                    throw new NotFoundException("Influencer Not Found");
                }

                entity.Update(
                    dto.BoothId,
                    dto.Name,
                    dto.CategoryId,
                    dto.CastId,
                    dto.Mobile,
                    dto.Description,
                    dto.VillageIds
                );

                return _repo.Update(entity);
            }
        }
    }
}
