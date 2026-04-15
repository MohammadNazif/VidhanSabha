using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.Interfaces;

namespace VidhanSabha.Application.Pannels.SuperAdmin.designation.Commands
{
    internal class UpdateDesignationCommandHandler : IRequestHandler<UpdateDesignationCommand, int>
    {
        private IDesignationRepository _repo;

        public UpdateDesignationCommandHandler(IDesignationRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdateDesignationCommand request, CancellationToken cancellationToken)
        {
              var req = request.Dto;
              var designation =  await _repo.GetByIdAsync(req.Id);
              designation.Update(req.DesignationName, req.DesignationTypeId);
               return await _repo.UpdateAsync(designation, cancellationToken);
           
        }
    }
}
