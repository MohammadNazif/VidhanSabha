using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Command
{
    public class CreateBoothSamitiMemCommandHandler : IRequestHandler<CreateBoothSamitiMemCommand, int>
    {
        private readonly IBoothSamitiRepository _repository;

        public CreateBoothSamitiMemCommandHandler(IBoothSamitiRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
    CreateBoothSamitiMemCommand request,
    CancellationToken cancellationToken)
        {
            var entity = Tbl_BoothSamitiMem.Create(
                request.BoothId,
                request.UserId
            );

            return await _repository.AddAsync(entity, cancellationToken);
        }
    }
}
