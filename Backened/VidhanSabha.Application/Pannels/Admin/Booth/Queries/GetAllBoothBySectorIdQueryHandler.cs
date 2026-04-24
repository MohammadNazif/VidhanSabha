using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Queries;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{
    //public class GetAllBoothBySectorIdQueryHandler:IRequestHandler<GetAllBoothBySectorIdQuery,PagedResult<BoothResponseDto>>
    //{
    //    //private IBoothRepository _repo;

    //    //public GetAllBoothBySectorIdQueryHandler(IBoothRepository repo)
    //    //{
    //    //    _repo = repo;
    //    //}
    //    //public async Task<PagedResult<BoothResponseDto>> Handle(GetAllBoothBySectorIdQuery request, CancellationToken cancellationToken)
    //    //{
    //    //    var res = await _repo.GetBoothBySectorIdAsync(request.QueryParams);
    //    //    if (res == null)
    //    //    {
    //    //        throw new NotFoundException("Booth Not Found");
    //    //    }
    //    //    return res;
    //    //}
    //}
}
