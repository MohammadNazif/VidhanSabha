using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.DoubleVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.DoubleVoter.Command
{
    internal class CreateDoubleVoterCommandHandler : IRequestHandler<CreateDoubleVoterCommand, int>
    {
        private readonly IDoubleVoterRepository _repo;
        private readonly IBoothRepository _booth;

        public CreateDoubleVoterCommandHandler(IDoubleVoterRepository repo, IBoothRepository booth)
        {
            _repo = repo;
            _booth = booth;
        }
        public async Task<int> Handle(CreateDoubleVoterCommand request, CancellationToken cancellationToken)
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

            var data = Tbl_DoubleVoter.Create(
                req.BoothId, req.Name, req.FatherName,
                req.VoterId, req.PreviousAddress, req.CurrentAddress, req.Description, request.UserId, createdtouserId, req.VillageId);


            return await _repo.AddAsync(data, cancellationToken);

        }
        private bool IsUserRole(string currentRole, PrabhariRole roleToCheck)
        {
            return currentRole == roleToCheck.ToString();
        }
    }
}
