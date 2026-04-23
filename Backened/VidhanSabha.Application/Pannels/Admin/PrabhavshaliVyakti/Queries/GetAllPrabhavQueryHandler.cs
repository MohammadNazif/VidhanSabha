using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Queries;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Queries
{
    public class GetAllPrabhavQueryHandler : IRequestHandler<GetAllPrabhavQuery, PagedResult<PrabhavshaliResponseDto>>
    {
        private IPrabhavshaliRepository _repo;

        public GetAllPrabhavQueryHandler(IPrabhavshaliRepository repo)
        {
            _repo = repo;
        }
        public async Task<PagedResult<PrabhavshaliResponseDto>> Handle(GetAllPrabhavQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllAsync(request.QueryParams);
            if (res == null)
            {
                throw new NotFoundException("Prabhavshali Vyakti Not Found");
            }
            return res;
        }
    }
}
