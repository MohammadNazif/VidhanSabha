using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Commands
{
    public class DeleteSectorHandler : IRequestHandler<DeleteSectorCommand, bool>
    {
        private readonly ISectorRepository _repo;

        public DeleteSectorHandler(ISectorRepository repo) => _repo = repo;

        public async Task<bool> Handle(DeleteSectorCommand request, CancellationToken cancellationToken)
        {
            var sector = await _repo.GetByIdAsync(request.Id)
                ?? throw new NotFoundException($"Sector {request.Id} not found.");

            await _repo.DeleteAsync(sector);
            return true;
        }
    }
}
