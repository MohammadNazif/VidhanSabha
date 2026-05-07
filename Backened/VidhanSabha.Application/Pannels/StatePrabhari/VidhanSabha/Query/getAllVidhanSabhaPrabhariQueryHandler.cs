using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Interfaces;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Query
{
    internal class getAllVidhanSabhaPrabhariQueryHandler : IRequestHandler<getAllVidhanSabhaPrabhariQuery, PagedResult<StatePrabhariResponseDto>>
    {
        private IStatePrabhariRepository _repo;

        public getAllVidhanSabhaPrabhariQueryHandler(IStatePrabhariRepository repo)
        {
            _repo = repo;
        }
        public async Task<PagedResult<StatePrabhariResponseDto>> Handle(getAllVidhanSabhaPrabhariQuery request, CancellationToken cancellationToken)
        {
            var data = await _repo.GetByStateIdAsync(request.StateId,request.UserId,request.QP);
            if(data == null)
            {
                throw new NotFoundException("Not Found");
            }
            return data;
        }
    }
}
