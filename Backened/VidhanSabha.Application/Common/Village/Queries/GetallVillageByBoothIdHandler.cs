using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Category.Interfaces;
using VidhanSabha.Application.Common.Village.DTOs;

namespace VidhanSabha.Application.Common.Village.Queries
{
    public class GetallVillageByBoothIdHandler : IRequestHandler<GetallVillageByBoothId, List<VillageByBoothResponseDto>>
    {
        private IVillageRepository _repo;

        public GetallVillageByBoothIdHandler(IVillageRepository repo)
        {
            _repo = repo;
        }
        public Task<List<VillageByBoothResponseDto>> Handle(GetallVillageByBoothId request, CancellationToken cancellationToken)
        {
            var res = _repo.GetAllByBoothIdAsync(request.id);
             return res ;
        }
    }
}
