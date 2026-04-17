using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.District.DTOs;
using VidhanSabha.Application.Common.District.Interfaces;
using VidhanSabha.Application.Exceptions;

namespace VidhanSabha.Application.Common.District.Queries
{
    public class GetAllDistrictQueryHandler:IRequestHandler<GetAllDistrictQuery , List<DistrictResponseDto>>
    {
        private IDistrictRepository _repo;

        public GetAllDistrictQueryHandler(IDistrictRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<DistrictResponseDto>> Handle(GetAllDistrictQuery query,CancellationToken cancellationtoken)
        {
            var res = await _repo.GetDistrictsByIdAsync(query.Id);
            if(res==null)
            {
                throw new NotFoundException("District Not Found");
            }
            var DistrictResponse =  res.Select(x => new DistrictResponseDto
            {
                Id = x.Id,
                DistrictName = x.DistrictName,
                Status = x.Status,
            }).ToList();
            return DistrictResponse;
        }
    }
}
