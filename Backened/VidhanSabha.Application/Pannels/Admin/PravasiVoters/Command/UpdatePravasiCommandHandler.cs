using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command
{
    public class UpdatePravasiCommandHandler : IRequestHandler<UpdatePravasiCommand ,int>
    {
        private IPravasiVoterRepository _repo;

        public UpdatePravasiCommandHandler(IPravasiVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdatePravasiCommand request,CancellationToken cancellationtoken)
        {
            var dto = request.Dto;
            var pravasi = await _repo.GetByIdAsync(dto.Id);
            if(pravasi==null)
            {
                throw new NotFoundException("Pravasi Voter Not Found");
            }
              pravasi.Update(dto.BoothId, 
                  dto.Name, dto.Mobile, dto.CategoryId, 
                  dto.CastId, dto.OccupationId, dto.VoterId, 
                  dto.CurrentAddress, dto.VillageId);
            return _repo.Update(pravasi);
        }
    }
}

//{
//    "boothId": 1009,
//  "villageId": [
//    1013,1007
//  ],
//  "name": "bdfbd",
//  "mobile": "strdfgdfging",
//  "categoryId": 2,
//  "castId": 4,
//  "occupationId": 4,
//  "voterId": "strifvxvxg",
//  "currentAddress": "strinbvdxbvxvbg",
//  "id": 6
//}