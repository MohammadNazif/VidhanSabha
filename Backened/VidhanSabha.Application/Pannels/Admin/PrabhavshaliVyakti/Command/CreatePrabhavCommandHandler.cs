using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Command
{
    public class CreatePrabhavCommandHandler : IRequestHandler<CreatePrabhavCommand, int>
    {
        private readonly IBoothRepository _booth;
        private readonly IPrabhavshaliRepository _repo;

        public CreatePrabhavCommandHandler(IPrabhavshaliRepository repo, IBoothRepository booth)
        {
            _booth = booth;
            _repo = repo;
        }
        public async Task<int> Handle(CreatePrabhavCommand request, CancellationToken cancellationToken)
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


            var data = Tbl_PrabhavshaliVyakti.Create(
                req.BoothId, req.DesignationId, req.Name, req.CategoryId, req.CastId, req.Mobile, req.Description, request.UserId, createdtouserId, createdsectorUserId,request.Role,
                req.VillageId);

            return await _repo.AddAsync(data, cancellationToken);

        }
        private bool IsUserRole(string currentRole, PrabhariRole roleToCheck)
        {
            return currentRole == roleToCheck.ToString();
        }
    }
}
