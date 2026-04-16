using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Occupation.DTOs;
using VidhanSabha.Application.Common.Occupation.Interface;

namespace VidhanSabha.Application.Common.Occupation.Queries
{
    internal class GetAllOccupationQueryHandler:IRequestHandler<GetAllOccupationQuery,IReadOnlyList<OccupationResponseDto>>
    {
        private readonly IOccupationRepository _repo;

        public GetAllOccupationQueryHandler(IOccupationRepository repo)
        {
            _repo = repo;
        }
        public async Task<IReadOnlyList<OccupationResponseDto>> Handle(GetAllOccupationQuery request,CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllAsync();
            return res.Select(x => new OccupationResponseDto
            {
                Id = x.Id,
                Occupation = x.Occupation,
            }).ToList();
        }
    }
}
