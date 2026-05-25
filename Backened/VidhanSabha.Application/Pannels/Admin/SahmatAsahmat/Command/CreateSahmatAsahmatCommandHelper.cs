using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Command
{
    public class CreateSahmatAsahmatCommandHandler : IRequestHandler<CreateSahmatAsahmatCommand, int>
    {
        private readonly ISahmatAsahmatRepository _repo;
        private readonly IBoothRepository _booth;

        public CreateSahmatAsahmatCommandHandler(ISahmatAsahmatRepository repo,IBoothRepository booth)
        {
            _repo = repo;
            _booth = booth;
        }
        public async Task<int> Handle(CreateSahmatAsahmatCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;

            string createdtouserId = null;
            string createdsectorUserId = null;
            if (IsUserRole(request.Role, PrabhariRole.VidhanSabhaPrabhari))
            {
                createdsectorUserId = await _booth.GetSectorUseridbyBoothId(req.BoothId);
                createdtouserId = await _booth.GetUseridbyBoothId(req.BoothId);
            }
            else if (IsUserRole(request.Role, PrabhariRole.BoothSanyojak))
            {
                createdtouserId = request.UserId;
                createdsectorUserId = await _booth.GetSectorUseridbyBoothId(req.BoothId);

                request.UserId = await _booth.GetadminUseridbyUserId(req.BoothId);

            }
            else if (IsUserRole(request.Role, PrabhariRole.SectorSanyojak))
            {
                createdsectorUserId = request.UserId;
                createdtouserId = await _booth.GetUseridbyBoothId(req.BoothId);
                request.UserId = await _booth.GetadminUseridbyUserId(req.BoothId);

            }
          
            var data = Tbl_SahmatAsahmat.Create(
                req.BoothId,req.TypeId,
                req.Name,req.Age,req.Mobile,req.PartyId,
                req.OccupationId,req.Reason,req.VoterId, request.UserId, createdtouserId, createdsectorUserId,request.Role,
                req.VillageId
                );


            return await _repo.AddAsync(data, cancellationToken);



        }
        public bool IsUserRole(string userRole, PrabhariRole roleToCheck)
        {
            return string.Equals(userRole, roleToCheck.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
