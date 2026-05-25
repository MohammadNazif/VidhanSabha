using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Command
{
    internal class deleteMandalMemberCommandHandler : IRequestHandler<deleteMandalMemberCommand, int>
    {
        private IMandalSamiti _repo;

        public deleteMandalMemberCommandHandler(IMandalSamiti repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(deleteMandalMemberCommand request, CancellationToken cancellationToken)
        {
            var data = await _repo.GetMandalSamitiMemberByIdAsync(request.Id,cancellationToken);


            if(data == null)
            {
                throw new NotFoundException("Mandal Samiti Member Not Found");
            }

            var mandalSamiti = await _repo.GetMandalSamitiByIdAsync(
              request.MandalId, cancellationToken);

            data.Delete();
            mandalSamiti.Decrement();

            _repo.UpdateMandalSamitiMember(data);
            _repo.UpdateMandalSamiti(mandalSamiti);

            return  await _repo.SaveChangesAsync(cancellationToken);

        }
    }
}
