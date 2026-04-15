using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.Interfaces;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Application.Pannels.SuperAdmin.designation.Commands
{
    internal class CreateDesignationCommandHandler : IRequestHandler<CreateDesignationCommand, int>
    {
        private IDesignationRepository _repo;

        public CreateDesignationCommandHandler(IDesignationRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateDesignationCommand request, CancellationToken cancellationToken)
        {
            var data = Tbl_Designation.Create(request.Dto.DesignationName, request.Dto.DesignationTypeId);

            return  await _repo.CreateAsync(data, cancellationToken);
        }
    }
}
