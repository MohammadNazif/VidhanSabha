using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.BoothSamiti.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Command
{
    public class CreateBoothSamitiMemCommandHandler : IRequestHandler<CreateBoothSamitiMemCommand, int>
    {
        private readonly IBoothSamitiRepository _repository;
        private readonly IBoothRepository _booth;

        public CreateBoothSamitiMemCommandHandler(IBoothSamitiRepository repository, IBoothRepository booth)
        {
            _repository = repository;
            _booth = booth;
        }

        public async Task<int> Handle(
    CreateBoothSamitiMemCommand request,
    CancellationToken cancellationToken)
        {

            string createdtouserId = null;
            string createdsectorUserId = null;
            if (IsUserRole(request.Role, PrabhariRole.VidhanSabhaPrabhari))
            {
                createdsectorUserId = await _booth.GetSectorUseridbyBoothId(request.BoothId);
                createdtouserId = await _booth.GetUseridbyBoothId(request.BoothId);
            }
            else if (IsUserRole(request.Role, PrabhariRole.BoothSanyojak))
            {
                createdtouserId = request.UserId;
                createdsectorUserId = await _booth.GetSectorUseridbyBoothId(request.BoothId);

                request.UserId = await _booth.GetadminUseridbyUserId(request.BoothId);

            }
            else if (IsUserRole(request.Role, PrabhariRole.SectorSanyojak))
            {
                createdsectorUserId = request.UserId;
                createdtouserId = await _booth.GetUseridbyBoothId(request.BoothId);
                request.UserId = await _booth.GetadminUseridbyUserId(request.BoothId);

            }
            var entity = Tbl_BoothSamitiMem.Create(
                request.BoothId,
                request.UserId,
                createdtouserId,
                createdsectorUserId,
                request.Role


            );

            return await _repository.AddAsync(entity, cancellationToken);
        }
        public bool IsUserRole(string userRole, PrabhariRole roleToCheck)
        {
            return string.Equals(userRole, roleToCheck.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}