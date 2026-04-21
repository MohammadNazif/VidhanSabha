using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Command
{
    public class UpdateSeniorDisabledCommandHandler : IRequestHandler<UpdateSeniorDisabledCommand, int>
    {
        private ISeniorDisabledRepository _repo;
        public UpdateSeniorDisabledCommandHandler(ISeniorDisabledRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdateSeniorDisabledCommand request, CancellationToken cancellationtoken)
        {
            var dto = request.Dto;
            var seniordisabled = await _repo.GetByIdAsync(dto.Id);
            if (seniordisabled == null)
            {
                throw new NotFoundException("Senior/Disabled Not Found");
            }
            seniordisabled.Update(
                dto.TypeId, dto.BoothId, dto.Name, dto.Address, dto.CategoryId,
                dto.CastId, dto.Mobile, dto.VoterId, dto.VillageId);
            return _repo.Update(seniordisabled);
        }
    }
}
