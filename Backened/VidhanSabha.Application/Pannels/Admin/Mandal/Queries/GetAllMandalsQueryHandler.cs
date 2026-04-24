using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Queries
{
    public class GetAllMandalsQueryHandler
        : IRequestHandler<GetAllMandalsQuery, PagedResult<MandalResponseDto>>
    {
        private readonly IMandalRepository _repo;

        public GetAllMandalsQueryHandler(IMandalRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<MandalResponseDto>> Handle(
            GetAllMandalsQuery query, CancellationToken ct)
        {
           int? VidhanSabhaId =  await  _repo.GetVidhansabhaIdByuserIdAsync(query.UserId);
            var mandals = await _repo.GetAllAsync(query.QueryParams, VidhanSabhaId, ct);

            if (mandals == null)
            {
                throw new NotFoundException("No Mandals found.");
            }
                
            return mandals;
            
        }

        
    }
}
