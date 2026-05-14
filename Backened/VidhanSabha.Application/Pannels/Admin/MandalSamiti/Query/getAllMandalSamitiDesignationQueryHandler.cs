using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Dtos;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Query
{
    internal class getAllMandalSamitiDesignationQueryHandler : IRequestHandler<getAllMandalSamitiDesignationQuery, List<MandalSamitiDesignationResponseDto>>
    {
        private IMandalSamiti _repo;

        public getAllMandalSamitiDesignationQueryHandler(IMandalSamiti repo)
        {
            _repo = repo;
        }
        public Task<List<MandalSamitiDesignationResponseDto>> Handle(getAllMandalSamitiDesignationQuery request, CancellationToken cancellationToken)
        {
           return _repo.GetMandalSamitiDesignationAsync();
        }
    }
}
