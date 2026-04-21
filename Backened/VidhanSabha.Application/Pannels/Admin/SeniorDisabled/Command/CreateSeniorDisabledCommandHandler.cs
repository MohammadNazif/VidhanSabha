using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.ValueObjects;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Command
{
    public class CreateSeniorDisabledCommandHandler : IRequestHandler<CreateSeniorDisabledCommand, int>
    {
        private readonly ISeniorDisabledRepository _repo;

        public CreateSeniorDisabledCommandHandler(ISeniorDisabledRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateSeniorDisabledCommand request, CancellationToken cancellationToken)
        {

            var req = request.Dto;

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
        req.VillageId
       ))
    .ToList();

            return await _repo.AddAsync(entities, cancellationToken);
        }
    }
}
