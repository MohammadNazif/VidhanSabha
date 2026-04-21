using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.BDC.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Command
{
    public class UpdateBDCCommandHandler : IRequestHandler<UpdateBDCCommand, int>
    {
        private IBDCRepository _repo;

        public UpdateBDCCommandHandler(IBDCRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdateBDCCommand request, CancellationToken cancellationtoken)
        {
            var dto = request.Dto;
            var bdc = await _repo.GetByIdAsync(dto.Id);
            if (bdc == null)
            {
                throw new NotFoundException("BDC Not Found");
            }
            bdc.Update(dto.Block,
                dto.Name,dto.WardNumber,dto.CategoryId,dto.CastId,dto.Age,dto.Mobile,
                dto.PartyId,dto.Education,dto.VillageId);
            return _repo.Update(bdc);
        }
    }
}
