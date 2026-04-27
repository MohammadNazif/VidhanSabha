using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{

    public class GetAllBoothsQueryHandler : IRequestHandler<GetAllBoothsQuery, PagedResult<BoothResponseDto>>
    {
        private readonly IBoothRepository _repo;
        private readonly IMandalRepository _man;

        public GetAllBoothsQueryHandler(IBoothRepository repo,IMandalRepository man) { _repo = repo; _man = man; }

        public async Task<PagedResult<BoothResponseDto>> Handle(GetAllBoothsQuery q, CancellationToken ct)
        {

            int? vidhanId = await _man.GetVidhansabhaIdByuserIdAsync(q.userId);
            var list = await _repo.GetAllAsync(q.QueryParams, vidhanId, ct);
            return list;
        }
    

    }
}
