using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.BoothSamitiDesignation.DTOs;
using VidhanSabha.Application.Common.BoothSamitiDesignation.Interfaces;

namespace VidhanSabha.Application.Common.BoothSamitiDesignation.Queries
{
    public class GetAllBoothSamitiDesignationHandler
       : IRequestHandler<GetAllBoothSamitiDesignationQuery, List<DesignationDto>>
    {
        private readonly IBoothSamitiDesignationRepository _repository;

        public GetAllBoothSamitiDesignationHandler(
            IBoothSamitiDesignationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DesignationDto>> Handle(
            GetAllBoothSamitiDesignationQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }
    }
}
