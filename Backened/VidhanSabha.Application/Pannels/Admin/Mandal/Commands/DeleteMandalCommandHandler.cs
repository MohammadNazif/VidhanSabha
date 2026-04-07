using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Commands
{
    public class DeleteMandalCommandHandler : IRequestHandler<DeleteMandalCommand, int>
    {
        private IMandalRepository _repo;

        public DeleteMandalCommandHandler(IMandalRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteMandalCommand request, CancellationToken cancellationToken)
        {
             var mandal = await _repo.GetByIdAsync(request.Id);
             if (mandal == null) throw new NotFoundException("Mandal Not Found");

            mandal.Deactivate();

            await _repo.UpdateAsync(mandal);

            return mandal.Id;

        }
    }
}
