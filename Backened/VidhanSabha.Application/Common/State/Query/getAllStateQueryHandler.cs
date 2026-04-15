using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.State.Dto;
using VidhanSabha.Application.Common.State.Interface;



namespace VidhanSabha.Application.Common.State.Query
{
    internal class getAllStateQueryHandler : IRequestHandler<getAllStateQuery, IReadOnlyList<StateResponseDto>>
    {
        private IStateRepository _repo;

        public getAllStateQueryHandler(IStateRepository repo)
        {
            _repo = repo;
        }
        public async Task<IReadOnlyList<StateResponseDto>> Handle(getAllStateQuery request, CancellationToken cancellationToken)
        {
           var res = await _repo.getAllAsync();

            return res.Select(x => new StateResponseDto
            {
                Id = x.Id,
                StateName = x.StateName
            }).ToList();
        }
    }
}
