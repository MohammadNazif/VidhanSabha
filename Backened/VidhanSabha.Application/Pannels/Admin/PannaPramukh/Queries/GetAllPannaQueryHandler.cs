using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Queries
{
    public class GetAllPannaQueryHandler : IRequestHandler<GetAllPannaQuery, PagedResult<PannaPramukhResponseDto>>
    {
        private IPannaPramukhRepository _repo;

        public GetAllPannaQueryHandler(IPannaPramukhRepository repo)
        {
            _repo = repo;   
        }
        public async Task<PagedResult<PannaPramukhResponseDto>> Handle(GetAllPannaQuery request, CancellationToken cancellationToken)
        {
             var res =  await _repo.GetAllAsync(request.QueryParams);
            if(res == null)
            {
                throw new NotFoundException("Panna Pramukh Not Found");
            }
          
            return res;
        }
    }
}
