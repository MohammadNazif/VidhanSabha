using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Command
{
    public class DeleteBoothSamitiHandler
        : IRequestHandler<DeleteBoothSamitiCommand, int>
    {
        private readonly IBoothSamitiRepository _repository;

        public DeleteBoothSamitiHandler(IBoothSamitiRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
            DeleteBoothSamitiCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
                throw new Exception("Data not found");

            _repository.Delete(entity);

            return request.Id;
        }
    }
}
