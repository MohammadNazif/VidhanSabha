using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Queries
{
    public class GetAllBoothSamitiHandler
        : IRequestHandler<GetAllBoothSamitiQuery, List<BoothSamitiResponseDto>>
    {
        private readonly IBoothSamitiRepository _repository;

        public GetAllBoothSamitiHandler(IBoothSamitiRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BoothSamitiResponseDto>> Handle(
            GetAllBoothSamitiQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(request.Id, cancellationToken);
        }
    }
}
