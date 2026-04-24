using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command
{
    public class CreatePannaCommandHandler : IRequestHandler<CreatePannaCommand, int>
    {
        private IPannaPramukhRepository _repo;

        public CreatePannaCommandHandler(IPannaPramukhRepository repo)
        {
            _repo = repo;
        }
        public Task<int> Handle(CreatePannaCommand request, CancellationToken cancellationToken)
        {

          var pannapramukh =  Tbl_PannaPramukh.Create(
              request.Dto.BoothId, request.Dto.PannaNumber, 
              request.Dto.PannaPramukhName, request.Dto.CategoryId, 
              request.Dto.CastId, request.Dto.VoterId, 
              request.Dto.PhoneNumber, request.Dto.Address, request.UserId,
              request.Dto.VillageId);

            _repo.AddAsync(pannapramukh,cancellationToken);
   
            return Task.FromResult(pannapramukh.Id);

        }
    }
}
