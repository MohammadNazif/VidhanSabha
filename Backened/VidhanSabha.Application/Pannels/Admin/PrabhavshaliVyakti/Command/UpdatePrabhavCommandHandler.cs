using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Command
{
    public class UpdatePrabhavCommandHandler : IRequestHandler<UpdatePrabhavCommand, int>
    {
        private IPrabhavshaliRepository _repo;

        public UpdatePrabhavCommandHandler(IPrabhavshaliRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdatePrabhavCommand request, CancellationToken cancellationtoken)
        {
            var dto = request.Dto;
            var prabhav = await _repo.GetByIdAsync(dto.Id);
            if (prabhav == null)
            {
                throw new NotFoundException("Prabhavshali Vyakti Not Found");
            }
            prabhav.Update(
                dto.BoothId,dto.DesignationId,
                dto.Name, dto.CategoryId,
                dto.CastId, dto.Mobile,dto.Description,
                dto.VillageId);
            return _repo.Update(prabhav);
        }
    }
}
