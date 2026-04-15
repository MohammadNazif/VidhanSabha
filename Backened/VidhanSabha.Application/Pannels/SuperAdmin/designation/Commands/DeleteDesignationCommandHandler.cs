using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.Interfaces;

namespace VidhanSabha.Application.Pannels.SuperAdmin.designation.Commands
{
    internal class DeleteDesignationCommandHandler : IRequestHandler<DeleteDesignationCommand, int>
    {
        private IDesignationRepository _repo;

        public DeleteDesignationCommandHandler(IDesignationRepository repo)
        {
            _repo = repo; 
        }
        public async Task<int> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
        {
           var designation =  await _repo.GetByIdAsync(request.Id);
            designation.Delete();
          return  await _repo.UpdateAsync(designation);
        }
    }
}
