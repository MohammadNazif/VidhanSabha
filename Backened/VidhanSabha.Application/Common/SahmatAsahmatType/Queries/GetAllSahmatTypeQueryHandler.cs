using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.SahmatAsahmatType.DTOs;
using VidhanSabha.Application.Common.SahmatAsahmatType.Interfaces;
using VidhanSabha.Application.Exceptions;

namespace VidhanSabha.Application.Common.SahmatAsahmatType.Queries
{
    public class GetAllSahmatTypeQueryHandler:IRequestHandler<GetAllSahmatTypeQuery,List<SahmatTypeResponseDto>>
    {
        private ISahmatTypeRepository _repo;

        public GetAllSahmatTypeQueryHandler(ISahmatTypeRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<SahmatTypeResponseDto>> Handle(GetAllSahmatTypeQuery query,CancellationToken cancellationtiontoken)
        {
            var res = await _repo.GetAllAsync();
            if(res==null)
            {
                throw new NotFoundException("Type Not Found");
            }
            var result=res.Select(x => new SahmatTypeResponseDto
            {
                Id = x.Id,
                Type = x.Type,
            }).ToList();
            return result;
        }
    }
}
