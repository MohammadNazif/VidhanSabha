using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Interface;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Query
{
    internal class getAllVidhanSabhaQueryHandler : IRequestHandler<getAllVidhanSabhaQuery, IReadOnlyList<VidhanSabhaSatewiseResponseDto>>
    {
        private IVidhanSabhaRepository _repo;

        public getAllVidhanSabhaQueryHandler(IVidhanSabhaRepository repo)
        {
            _repo = repo;
        }
            public async Task<IReadOnlyList<VidhanSabhaSatewiseResponseDto>> Handle(getAllVidhanSabhaQuery request, CancellationToken cancellationToken)
        {
           
            return await _repo.GetByIdAsync(request.q,request.districtId);
        }
    }
}


