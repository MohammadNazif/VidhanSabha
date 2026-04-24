using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Queries
{
    public class GetAllSeniorDisabledQueryHandler : IRequestHandler<GetAllSeniorDisabledQuery, PagedResult<SeniorDisabledResponseDto>>
    {
        private ISeniorDisabledRepository _repo;

        public GetAllSeniorDisabledQueryHandler(ISeniorDisabledRepository repo)
        {
            _repo = repo;
        }
        public async Task<PagedResult<SeniorDisabledResponseDto>> Handle(GetAllSeniorDisabledQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllAsync(request.QueryParams);
            if (res == null)
            {
                throw new NotFoundException("Senior/Disabled Not Found");
            }
            return res;
        }
    }
}
