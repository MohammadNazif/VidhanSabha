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
            if (IsUserRole(request.Role, PrabhariRole.VidhanSabhaPrabhari))
            {
                createdtouserId = await _booth.GetUseridbyBoothId(request.Dto.BoothId);
            }
            else if (IsUserRole(request.Role, PrabhariRole.BoothSanyojak))
            {
                createdtouserId = request.UserId;

                request.UserId = await _booth.GetadminUseridbyUserId(request.Dto.BoothId);

            }



            var data = Tbl_PrabhavshaliVyakti.Create(
                req.BoothId, req.DesignationId, req.Name, req.CategoryId, req.CastId, req.Mobile, req.Description, request.UserId, createdtouserId,
                req.VillageId);

            return await _repo.AddAsync(data, cancellationToken);

        }
        private bool IsUserRole(string currentRole, PrabhariRole roleToCheck)
        {
            return currentRole == roleToCheck.ToString();
        }
    }
}
