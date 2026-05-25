using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Dtos;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Query
{
    internal class getAllMandalSamitiQueryHandler : IRequestHandler<getAllMandalSamitiQuery, PagedResult<MandalSamitiResponseDto>>
    {
        private IMandalSamiti _repo;
        public getAllMandalSamitiQueryHandler(IMandalSamiti repo)
        {
            _repo = repo;
        }
        public Task<PagedResult<MandalSamitiResponseDto>> Handle(getAllMandalSamitiQuery request, CancellationToken cancellationToken)
        {
            return _repo.GetAllMandalSamitiAsync(request.Dto,cancellationToken);
        }
    }
}
