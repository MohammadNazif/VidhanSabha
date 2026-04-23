using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.Command
{
    public class UpdateBoothVoterCommandHelper : IRequestHandler<UpdateBoothVoterCommand, int>
    {
        private IBoothVoterRepository _repo;

        public UpdateBoothVoterCommandHelper(IBoothVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdateBoothVoterCommand request, CancellationToken cancellationtoken)
        {
            var dto = request.Dto;
            var boothvoter = await _repo.GetByIdAsync(dto.Id);
            if (boothvoter == null)
            {
                throw new NotFoundException("Booth Voter Not Found");
            }
            boothvoter.Update(dto.BoothId,
                  dto.TotalVoter, dto.Male, dto.Female, dto.Other, dto.VillageId);
            return _repo.Update(boothvoter);
        }
    }
}
