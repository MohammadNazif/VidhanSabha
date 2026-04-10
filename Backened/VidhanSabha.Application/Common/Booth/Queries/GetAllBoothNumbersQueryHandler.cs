using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;

namespace VidhanSabha.Application.Common.Booth.Queries
{
    public class GetAllBoothNumbersQueryHandler : IRequestHandler<GetAllBoothNumbersQuery, List<BoothNumberDto>>
    {
        private IBoothRepository _repo;

        public GetAllBoothNumbersQueryHandler(IBoothRepository repo)
        {
            _repo = repo;
        }
        public Task<List<BoothNumberDto>> Handle(GetAllBoothNumbersQuery request, CancellationToken cancellationToken)
        {
            return _repo.BoothNumberExistsAsync();
             
        }
    }
}
