using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.Command
{
    public class CreateBoothVoterCommandHandler : IRequestHandler<CreateBoothVoterCommand, int>
    {
        private readonly IBoothVoterRepository _repo;
        private readonly IBoothRepository _booth;

        public CreateBoothVoterCommandHandler(IBoothVoterRepository repo,IBoothRepository booth)
        {
            _repo = repo;
            _booth = booth;
        }
        public async Task<int> Handle(CreateBoothVoterCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;
            string createdtouserId = null;
            string createdsectorUserId = null;
            if (IsUserRole(request.Role, PrabhariRole.VidhanSabhaPrabhari))
            {
                createdsectorUserId = await _booth.GetSectorUseridbyBoothId(request.Dto.BoothId);
                createdtouserId = await _booth.GetUseridbyBoothId(request.Dto.BoothId);
            }
            else if (IsUserRole(request.Role, PrabhariRole.BoothSanyojak))
            {
                createdtouserId = request.UserId;
                createdsectorUserId = await _booth.GetSectorUseridbyBoothId(request.Dto.BoothId);

                request.UserId = await _booth.GetadminUseridbyUserId(request.Dto.BoothId);

            }
            else if (IsUserRole(request.Role, PrabhariRole.SectorSanyojak))
            {
                createdsectorUserId = request.UserId;
                createdtouserId = await _booth.GetUseridbyBoothId(request.Dto.BoothId);
                request.UserId = await _booth.GetadminUseridbyUserId(request.Dto.BoothId);

            }

            var data = Tbl_BoothVoter.Create(
                req.BoothId, req.TotalVoter, req.Male, req.Female, req.Other, request.UserId,createdtouserId,createdsectorUserId, request.Role, req.VillageId);
            return await _repo.AddAsync(data, cancellationToken);
        }

        private bool IsUserRole(string userRole, PrabhariRole role)
        {
            return string.Equals(userRole, role.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
