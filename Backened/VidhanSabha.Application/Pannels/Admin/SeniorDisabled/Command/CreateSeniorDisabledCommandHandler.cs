using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;
using VidhanSabha.Domain.ValueObjects;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Command
{
    public class CreateSeniorDisabledCommandHandler : IRequestHandler<CreateSeniorDisabledCommand, int>
    {
        private readonly ISeniorDisabledRepository _repo;
        private readonly IBoothRepository _booth;

        public CreateSeniorDisabledCommandHandler(ISeniorDisabledRepository repo,IBoothRepository booth)
        {
            _repo = repo;
            _booth = booth;
        }
        public async Task<int> Handle(CreateSeniorDisabledCommand request, CancellationToken cancellationToken)
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

            var dataList = req.SeniorDisabledRequest
        .Select(x => new SeniorDisabledData(
         x.Name,
         x.Address,
         x.CategoryId,
         x.CastId,
         x.Mobile,
         x.VoterId
     ))
     .ToList();

       var entities = dataList
       .Select(data => Tbl_SeniorDisabled.Create(
        req.TypeId,
        req.BoothId,
        data,
        request.UserId,
        createdtouserId,
        req.VillageId
       ))
    .ToList();

            return await _repo.AddAsync(entities, cancellationToken);
        }
        private bool IsUserRole(string currentRole, PrabhariRole roleToCheck)
        {
            return currentRole == roleToCheck.ToString();
        }
}
}
