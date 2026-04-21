using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Queries;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Queries
{
    public class GetAllSeniorDisabledQueryHandler : IRequestHandler<GetAllSeniorDisabledQuery, List<SeniorDisabledResponseDto>>
    {
        private ISeniorDisabledRepository _repo;

        public GetAllSeniorDisabledQueryHandler(ISeniorDisabledRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<SeniorDisabledResponseDto>> Handle(GetAllSeniorDisabledQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetAllAsync();
            if (res == null)
            {
                throw new NotFoundException("Senior/Disabled Not Found");
            }
            return res;
        }
    }
}
