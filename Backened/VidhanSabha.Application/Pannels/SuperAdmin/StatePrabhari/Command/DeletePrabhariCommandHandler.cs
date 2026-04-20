using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Interfaces;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command
{
    internal class DeletePrabhariCommandHandler : IRequestHandler<DeletePrabhariCommand,int>
    {
        private IStatePrabhariRepository _repo;
        private CredentialManagerFunc _cred;

        public DeletePrabhariCommandHandler(IStatePrabhariRepository repo,CredentialManagerFunc cred)
        {
            _repo = repo;
            _cred = cred;
        }

        public async Task<int> Handle(DeletePrabhariCommand request, CancellationToken cancellationToken)
        {
           var data = await _repo.GetByIdAsync(request.Id);

            if(data== null)
            {
                throw new NotFoundException("State Prabhari NotFound");
            }

             await _cred.Delete(request.UserId);

              data.Delete();
             return await _repo.UpdateAsync(data);
        }
    }
}
