using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.BDC.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Command
{
    public class DeleteBDCCommandHandler : IRequestHandler<DeleteBDCCommand, int>
    {
        private IBDCRepository _repo;

        public DeleteBDCCommandHandler(IBDCRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(DeleteBDCCommand request, CancellationToken cancellationtoken)
        {
            var bdc = await _repo.GetByIdAsync(request.Id);

            if (bdc == null)
            {
                throw new NotFoundException("BDC Not Found");
            }
            bdc.Delete();
            return _repo.Update(bdc);
        }
    }
}
