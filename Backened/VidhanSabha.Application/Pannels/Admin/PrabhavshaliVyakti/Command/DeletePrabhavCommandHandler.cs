using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Command
{
    public class DeletePrabhavCommandHandler : IRequestHandler<DeletePrabhavCommand, int>
    {
        private IPrabhavshaliRepository _repo;

        public DeletePrabhavCommandHandler(IPrabhavshaliRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeletePrabhavCommand request, CancellationToken cancellationtoken)
        {
            var prabhav = await _repo.GetByIdAsync(request.Id);

            if (prabhav == null)
            {
                throw new NotFoundException("Prabhavshali Vyakti Not Found");
            }
            prabhav.Delete();
            return _repo.Update(prabhav);
        }
    }
}
