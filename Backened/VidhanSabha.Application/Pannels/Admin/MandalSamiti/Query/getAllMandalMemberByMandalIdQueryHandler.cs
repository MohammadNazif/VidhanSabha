using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Dtos;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Query
{
    internal class getAllMandalMemberByMandalIdQueryHandler : IRequestHandler<getAllMandalMemberByMandalIdQuery, List<MandalSamitiMemberResponseDto>>
    {
        private IMandalSamiti _repo;

        public getAllMandalMemberByMandalIdQueryHandler(IMandalSamiti repo)
        {
            _repo = repo;  
        }
        public Task<List<MandalSamitiMemberResponseDto>> Handle(getAllMandalMemberByMandalIdQuery request, CancellationToken cancellationToken)
        {
           return _repo.GetAllMandalSamitiMemberByIdAsync(request.MandalId,cancellationToken);
           
        }
    }
}
