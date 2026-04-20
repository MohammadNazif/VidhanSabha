using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Queries;
using VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs;
using VidhanSabha.Application.Pannels.Admin.Pradhan.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.Queries
{
    public class GetAllPradhanQueryHandler : IRequestHandler<GetAllPradhanQuery, List<PradhanResponseDto>>
    {
        private IPradhanRepository _repo;

        public GetAllPradhanQueryHandler(IPradhanRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<PradhanResponseDto>> Handle(GetAllPradhanQuery query, CancellationToken cancellationtoken)
        {
            var res = await _repo.GetAllAsync();
            if (res == null)
            {
                throw new NotFoundException("New Voter Not Found");
            }
            return res;
        }
    }
}
