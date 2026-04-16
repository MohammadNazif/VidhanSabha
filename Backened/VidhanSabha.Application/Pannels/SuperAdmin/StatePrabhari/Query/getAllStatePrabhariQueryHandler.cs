using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Interfaces;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Query
{
    internal class getAllStatePrabhariQueryHandler : IRequestHandler<getAllStatePrabhariQuery, IReadOnlyList<StatePrabhariResponseDto>>
    {
        private IStatePrabhariRepository _repo;

        public getAllStatePrabhariQueryHandler(IStatePrabhariRepository repo)
        {
            _repo = repo;
        }
        public async  Task<IReadOnlyList<StatePrabhariResponseDto>> Handle(getAllStatePrabhariQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
