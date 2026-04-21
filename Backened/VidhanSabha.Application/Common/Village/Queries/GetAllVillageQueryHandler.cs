using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Category.Interfaces;
using VidhanSabha.Application.Common.Village.DTOs;
using VidhanSabha.Application.Exceptions;

namespace VidhanSabha.Application.Common.Village.Queries
{
    internal class GetAllVillageQueryHandler : IRequestHandler<GetAllVillageQuery, List<VillageResponseDtos>>
    {
        private IVillageRepository _repo;

        public GetAllVillageQueryHandler(IVillageRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<VillageResponseDtos>> Handle(GetAllVillageQuery request, CancellationToken cancellationToken)
        {
            var villages = await _repo.GetAllAsync();

            if (villages == null)
            {
                throw new NotFoundException("Villages Not Found");
            }

            return await _repo.GetAllVillageAsync();
        }
    }
}
