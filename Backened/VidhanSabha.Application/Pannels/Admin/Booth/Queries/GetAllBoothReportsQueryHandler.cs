using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{
    public class GetAllBoothReportsQueryHandler:IRequestHandler<GetAllBoothReportsQuery, PagedResult<BoothReportsDto>>
    {
        private IBoothRepository _repo;
        private IMandalRepository _man;

        public GetAllBoothReportsQueryHandler(IBoothRepository repo, IMandalRepository man)
        {
            _repo = repo;
            _man = man;
        }
        public async Task<PagedResult<BoothReportsDto>> Handle(GetAllBoothReportsQuery request , CancellationToken cancellationToken)
        {
            int? vidhanId = await _man.GetVidhansabhaIdByuserIdAsync(request.userId);
            var booth = await _repo.GetAllBoothReports(request.QueryParams,vidhanId,cancellationToken);
            if (booth == null)
            {
                throw new NotFoundException("Booth Not Found");
            }

            return booth;
        }
    }
}
