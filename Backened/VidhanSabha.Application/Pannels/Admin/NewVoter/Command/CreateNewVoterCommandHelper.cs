using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Command
{
    public class CreateNewVoterCommandHandler : IRequestHandler<CreateNewVoterCommand, int>
    {
        private readonly INewVoterRepository _repo;
        private readonly IBoothRepository _booth;

        public CreateNewVoterCommandHandler(INewVoterRepository repo, IBoothRepository booth)
        {
            _repo = repo;
            _booth = booth;
        }
        public async Task<int> Handle(CreateNewVoterCommand request, CancellationToken cancellationToken)
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

                var data = Tbl_NewVoter.Create(
                    req.BoothId, req.Name, req.FatherName, req.Mobile, req.CategoryId, req.CastId, req.DOB, req.Age,
                    req.VoterId, request.UserId, createdtouserId, createdsectorUserId,request.Role, req.VillageId);


            return await _repo.AddAsync(data, cancellationToken);



        }
        private bool IsUserRole(string currentRole, PrabhariRole roleToCheck)
        {
            return currentRole == roleToCheck.ToString();
        }

    }
}
