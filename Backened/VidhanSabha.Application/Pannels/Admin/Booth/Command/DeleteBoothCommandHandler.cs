using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Command
{
    public class DeleteBoothCommandHandler : IRequestHandler<DeleteBoothCommand>
    {
        private IBoothRepository _repo;

        public DeleteBoothCommandHandler(IBoothRepository repo)
        {
            _repo = repo;
        }
        public async Task Handle(DeleteBoothCommand request, CancellationToken cancellationToken)
        {
            var booth = await _repo.GetByIdAsync(request.Id)
                ?? throw new NotFoundException("Booth Not Found");
             booth.Delete();
            var res = _repo.Delete(booth);
           

        }
    }
}
