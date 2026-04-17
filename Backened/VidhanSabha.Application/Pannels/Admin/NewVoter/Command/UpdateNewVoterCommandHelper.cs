using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Command
{
    public class UpdateNewVoterCommandHelper:IRequestHandler<UpdateNewVoterCommand,int>
    {
        private  INewVoterRepository _repo;

        public UpdateNewVoterCommandHelper(INewVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdateNewVoterCommand request,CancellationToken cancellationtoken)
        {
            var dto = request.Dto;
            var newvoter = await _repo.GetByIdAsync(dto.Id);
            if(newvoter == null)
            {
                throw new NotFoundException("New Voter Not Found");
            }
            newvoter.Update(dto.BoothId,
                  dto.Name, dto.FatherName, dto.Mobile, dto.CategoryId, dto.CastId, dto.DOB,
                  dto.Age, dto.VoterId,
                  dto.VillageId);
            return _repo.Update(newvoter);
        }
    }
}
