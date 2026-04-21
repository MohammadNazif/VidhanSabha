using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.BDC.DTOs;
using VidhanSabha.Application.Pannels.Admin.BDC.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Queries;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Queries
{
    public class GetAllBDCQueryHandler : IRequestHandler<GetAllBDCQuery, List<BDCResponseDto>>
    {
        private IBDCRepository _repo;

        public GetAllBDCQueryHandler(IBDCRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<BDCResponseDto>> Handle(GetAllBDCQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllAsync();
            if (res == null)
            {
                throw new NotFoundException("BDC Not Found");
            }
            return res;
        }
    }
}
