using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Command
{
    public class UpdateBoothSamitiHandler
         : IRequestHandler<UpdateBoothSamitiCommand, int>
    {
        private readonly IBoothSamitiRepository _repository;

        public UpdateBoothSamitiHandler(IBoothSamitiRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
            UpdateBoothSamitiCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = await _repository.GetByIdAsync(dto.Id);

            if (entity == null)
                throw new Exception("Data not found");

            entity.Update(
            dto.Name,
            dto.CategoryId,
            dto.CasteId,
            dto.Age,
            dto.Contact,
            dto.Occupation,
            dto.DesignationId
        );

            return _repository.Update(entity);
        }
    }
}
