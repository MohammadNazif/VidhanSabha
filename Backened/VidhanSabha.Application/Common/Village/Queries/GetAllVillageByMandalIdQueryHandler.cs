using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Category.DTOs;
using VidhanSabha.Application.Common.Category.Interfaces;
using VidhanSabha.Application.Common.Village.DTOs;
using VidhanSabha.Application.Exceptions;

namespace VidhanSabha.Application.Common.Category.Queries
{
    public class GetAllVillageByMandalIdQueryHandler : IRequestHandler<GetallVillageByMandalId, List<VillageResponseDto>>
    {
        private IVillageRepository _repo;

        public GetAllVillageByMandalIdQueryHandler(IVillageRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<VillageResponseDto>> Handle(GetallVillageByMandalId request, CancellationToken cancellationToken)
        {
            var villages =  await _repo.GetAllByMandalIdAsync(request.id);

            if (villages == null || !villages.Any()) 
                 throw new NotFoundException("Village Not Found"); 
           
            
            return villages.Select(c => new VillageResponseDto
            {
                Id = c.Id,
                MandalId = c.MandalId,
                Name = c.VillageName,
                Status = c.Status
            }).ToList();
        }
    }
}
