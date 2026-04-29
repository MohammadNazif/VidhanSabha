using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Queries
{
    using MediatR;
    using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos;
    using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;

    public class GetBoothByIdHandler
        : IRequestHandler<GetBoothByIdQuery, BoothSamitiMemResponseDto>
    {
        private readonly IBoothSamitiRepository _repository;

        public GetBoothByIdHandler(IBoothSamitiRepository repository)
        {
            _repository = repository;
        }

        public async Task<BoothSamitiMemResponseDto> Handle(
            GetBoothByIdQuery request,
            CancellationToken cancellationToken)
        {
            var data = await _repository.GetBoothByIdAsync(request.BoothId, cancellationToken);

            if (data == null)
                throw new Exception("Booth not found");

            return data;
        }
    }
}
