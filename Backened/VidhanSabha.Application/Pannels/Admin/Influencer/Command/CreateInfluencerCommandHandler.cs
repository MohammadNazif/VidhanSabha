using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Influencer.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VidhanSabha.Application.Pannels.Admin.Influencer.Command
{
    namespace VidhanSabha.Application.Pannels.Admin.Influencer.Command
    {
        public class CreateInfluencerCommandHandler : IRequestHandler<CreateInfluencerCommand, int>
        {
            private readonly IInfluencerRepository _repo;

            public CreateInfluencerCommandHandler(IInfluencerRepository repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(CreateInfluencerCommand request, CancellationToken cancellationToken)
            {
                var influencer = Tbl_Influencer.Create(
                    request.Dto.BoothId,
                    request.Dto.Name,
                    request.Dto.CategoryId,
                    request.Dto.CastId,
                    request.Dto.Mobile,
                    request.Dto.Description,
                    request.UserId,
                    request.Dto.VillageIds
                );

                return await _repo.AddAsync(influencer, cancellationToken);
            }
        }
    }
}
