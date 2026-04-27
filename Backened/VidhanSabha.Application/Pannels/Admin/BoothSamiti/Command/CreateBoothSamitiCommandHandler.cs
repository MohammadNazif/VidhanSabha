using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Command
{
    public class CreateBoothSamitiHandler: IRequestHandler<CreateBoothSamitiCommand, int>
    {
        private readonly IBoothSamitiRepository _repository;

        public CreateBoothSamitiHandler(IBoothSamitiRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(
    CreateBoothSamitiCommand request,
    CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var entity = Tbl_BoothSamiti.Create(
                dto.Name,
                dto.CategoryId,
                dto.CasteId,
                dto.Age,
                dto.Contact,
                dto.Occupation,
                dto.DesignationId,
                request.UserId
            );

            return await _repository.AddAsync(entity, cancellationToken);
        }
    }
}
