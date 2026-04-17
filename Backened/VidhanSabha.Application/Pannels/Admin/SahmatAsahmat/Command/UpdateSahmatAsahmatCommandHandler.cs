using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Command
{
    public class UpdateSahmatAsahmatCommandHandler:IRequestHandler<UpdateSahmatAsahmatCommand,int>
    {
        private ISahmatAsahmatRepository _repo;

        public UpdateSahmatAsahmatCommandHandler(ISahmatAsahmatRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdateSahmatAsahmatCommand request, CancellationToken cancellationtoken)
        {
            var dto = request.Dto;
            var res = await _repo.GetByIdAsync(dto.Id);
            if(res==null)
            {
                throw new NotFoundException("Sahmat/Asahmat Voter Not Found");
            }
            res.Update(
                dto.BoothId, dto.TypeId, dto.IsAsahmat,
                dto.Name, dto.Age, dto.Mobile, dto.PartyId,
                dto.OccupationId, dto.Reason, dto.VoterId, dto.VillageId
                );
            return _repo.Update(res);
        }
    }
}
