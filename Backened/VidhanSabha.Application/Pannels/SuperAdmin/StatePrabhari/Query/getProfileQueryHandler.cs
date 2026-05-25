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
    internal class getProfileQueryHandler : IRequestHandler<getProfileQuery, StatePrabhariResponseDto>
    {
        private IStatePrabhariRepository _repo;


        public getProfileQueryHandler(IStatePrabhariRepository repo)
        {
            _repo = repo;
        }
        public Task<StatePrabhariResponseDto> Handle(getProfileQuery request, CancellationToken cancellationToken)
        {
            return _repo.GetProfileByUserIdAsync(request.UserId,request.Role);
        }
    }
}
