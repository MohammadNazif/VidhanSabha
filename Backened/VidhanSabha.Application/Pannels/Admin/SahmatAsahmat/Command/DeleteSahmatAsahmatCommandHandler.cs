using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Command
{
    internal class DeleteSahmatAsahmatCommandHandler: IRequestHandler<DeleteSahmatAsahmatCommand, int>
    {
        private readonly ISahmatAsahmatRepository _repo;

        public DeleteSahmatAsahmatCommandHandler(ISahmatAsahmatRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteSahmatAsahmatCommand request, CancellationToken cancellationtoken)
        {
            var res = await _repo.GetByIdAsync(request.Id);

            if (res == null)
            {
                throw new NotFoundException("Sahmat/Asahmat Voter Not Found");
            }
            res.Delete();
            return _repo.Update(res);
        }
    }
}
