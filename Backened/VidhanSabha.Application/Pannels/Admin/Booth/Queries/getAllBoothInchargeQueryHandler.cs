using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{
    internal class getAllBoothInchargeQueryHandler : IRequestHandler<getAllBoothInchargeQuery, IReadOnlyList<BoothInchargeResponse>>
    {
        private IBoothRepository _repo;

        public getAllBoothInchargeQueryHandler(IBoothRepository repo)
        {
            _repo = repo;
        }
        public async Task<IReadOnlyList<BoothInchargeResponse>> Handle(getAllBoothInchargeQuery request, CancellationToken cancellationToken)
        {
             return  await _repo.GetInchargeByBoothIdAsync(request.BoothId,request.UserId ,cancellationToken);
        }
    }
}
