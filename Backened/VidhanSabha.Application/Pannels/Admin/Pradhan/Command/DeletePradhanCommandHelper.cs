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
    public class DeletePradhanCommandHelper : IRequestHandler<DeletePradhanCommand, int>
    {
        private IPradhanRepository _repo;

        public DeletePradhanCommandHelper(IPradhanRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeletePradhanCommand request, CancellationToken cancellationtoken)
        {
            var pradhan = await _repo.GetByIdAsync(request.Id);
            if (pradhan == null)
            {
                throw new NotFoundException("Pradhan Not Found");
            }
            pradhan.Delete();
            return _repo.Update(pradhan);
        }
    }
}
