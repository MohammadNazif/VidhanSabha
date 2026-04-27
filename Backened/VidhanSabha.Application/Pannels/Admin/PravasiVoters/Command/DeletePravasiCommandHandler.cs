using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command
{
    public class DeletePravasiCommandHandler : IRequestHandler<DeletePravasiCommand, int>
    {
        private IPravasiVoterRepository _repo;

        public DeletePravasiCommandHandler(IPravasiVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeletePravasiCommand request,CancellationToken cancellationtoken)
        {
            var pravasi = await _repo.GetByIdAsync(request.Id);

            if(pravasi==null)
            {
                throw new NotFoundException("Pravasi Voter Not Found");
            }
            pravasi.Delete();
            return _repo.Update(pravasi);
        }
    }
}
