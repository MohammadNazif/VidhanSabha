using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Queries
{
    public class GetAllMemQueryHandler : IRequestHandler<GetAllMemQuery, PagedResult<BoothSamitiMemResponseDto>>
    {
        private readonly IBoothSamitiRepository _repository;

        public GetAllMemQueryHandler(IBoothSamitiRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<BoothSamitiMemResponseDto>> Handle(
            GetAllMemQuery request,
            CancellationToken cancellationToken)
        {
            var res = await _repository.GetAllMem(request.QueryParams);
            if (res == null)
            {
                throw new NotFoundException("Booth Mem Parent Data Not Found");
            }
            return res;
        }
    }
}
