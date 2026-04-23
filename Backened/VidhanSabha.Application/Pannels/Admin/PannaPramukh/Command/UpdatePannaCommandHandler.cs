using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command
{
    internal class UpdatePannaCommandHandler : IRequestHandler<UpdatePannaCommand, int>
    {
        private IPannaPramukhRepository _repo;

        public UpdatePannaCommandHandler(IPannaPramukhRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdatePannaCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
             var panna = await _repo.GetByIdAsync(dto.Id);
            if(panna == null)
            {
                throw new NotFoundException("Panna Pramukh Not Found");
            }

            panna.Update
                (dto.BoothId, dto.PannaNumber, dto.PannaPramukhName, dto.CategoryId,
                dto.CastId, dto.VoterId, dto.PhoneNumber, dto.Address, dto.VillageId);

             return _repo.Update(panna);

            
        }
    }
}
