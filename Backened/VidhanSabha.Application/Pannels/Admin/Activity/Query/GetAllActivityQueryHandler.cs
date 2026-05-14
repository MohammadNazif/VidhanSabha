
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Activity.Dtos;
using VidhanSabha.Application.Pannels.Admin.Activity.Interface;

namespace VidhanSabha.Application.Pannels.Admin.Activity.Query
{
   
    public class GetAllActivityQueryHandler : IRequestHandler<GetAllActivityQuery, PagedResult<ActivityResponseDto>>
    {
        private readonly IActivityRepository _repo;

        public GetAllActivityQueryHandler(IActivityRepository repo) => _repo = repo;

        public async Task<PagedResult<ActivityResponseDto>> Handle(GetAllActivityQuery request, CancellationToken ct)
        {
            var entities = await _repo.GetAllActiveAsync(request.qp,ct);

            return entities;
        }
    }
}
