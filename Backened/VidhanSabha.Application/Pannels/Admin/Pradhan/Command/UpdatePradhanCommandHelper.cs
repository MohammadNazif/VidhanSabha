using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Command;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Pradhan.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.Command
{
    public class UpdatePradhanCommandHelper : IRequestHandler<UpdatePradhanCommand, int>
    {
        private IPradhanRepository _repo;

        public UpdatePradhanCommandHelper(IPradhanRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdatePradhanCommand request, CancellationToken cancellationtoken)
        {
            var dto = request.Dto;
            if (!Enum.IsDefined(typeof(VidhanSabha.Domain.Enums.Gender), dto.Gender))
            {
                throw new Exception("Invalid Gender Value");
            }
            
            var pradhan = await _repo.GetByIdAsync(dto.Id);
            if (pradhan == null)
            {
                throw new NotFoundException("New Voter Not Found");
            }
            pradhan.Update(dto.Name, dto.DesignationId, dto.Contact, dto.Gender, dto.VillageId);
            return _repo.Update(pradhan);
        }
    }
}
