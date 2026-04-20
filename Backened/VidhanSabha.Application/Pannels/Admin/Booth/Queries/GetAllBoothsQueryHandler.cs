using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{

    public class GetAllBoothsQueryHandler : IRequestHandler<GetAllBoothsQuery, PagedResult<BoothResponseDto>>
    {
        private readonly IBoothRepository _repo;

        public GetAllBoothsQueryHandler(IBoothRepository repo) => _repo = repo;

        public async Task<PagedResult<BoothResponseDto>> Handle(GetAllBoothsQuery q, CancellationToken ct)
        {
            var list = await _repo.GetAllAsync(q.QueryParams, ct);
            return list;
        }
    

    }
}
