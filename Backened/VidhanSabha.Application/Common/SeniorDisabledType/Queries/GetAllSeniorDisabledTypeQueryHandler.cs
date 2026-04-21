using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.SahmatAsahmatType.DTOs;
using VidhanSabha.Application.Common.SahmatAsahmatType.Interfaces;
using VidhanSabha.Application.Common.SahmatAsahmatType.Queries;
using VidhanSabha.Application.Common.SeniorDisabledType.DTOs;
using VidhanSabha.Application.Common.SeniorDisabledType.Interfaces;
using VidhanSabha.Application.Exceptions;

namespace VidhanSabha.Application.Common.SeniorDisabledType.Queries
{
    public class GetAllSeniorDisabledTypeQueryHandler : IRequestHandler<GetAllSeniorDisabledTypeQuery, List<SeniorDisabledTypeResponseDto>>
    {
        private ISeniorDisabledTypeRepository _repo;

        public GetAllSeniorDisabledTypeQueryHandler(ISeniorDisabledTypeRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<SeniorDisabledTypeResponseDto>> Handle(GetAllSeniorDisabledTypeQuery query, CancellationToken cancellationtiontoken)
        {
            var res = await _repo.GetAllAsync();
            if (res == null)
            {
                throw new NotFoundException("Type Not Found");
            }
            var result = res.Select(x => new SeniorDisabledTypeResponseDto
            {
                Id = x.Id,
                Type = x.Type,
            }).ToList();
            return result;
        }
    }
}
