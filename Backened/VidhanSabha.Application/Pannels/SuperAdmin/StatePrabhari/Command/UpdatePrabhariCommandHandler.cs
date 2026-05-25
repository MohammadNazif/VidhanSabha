using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Common.CredentialMananger.Interface;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Interfaces;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command
{
    internal class UpdatePrabhariCommandHandler : IRequestHandler<UpdatePrabhariCommand, int>
    {
        private IStatePrabhariRepository _repo;
        private CredentialManagerFunc _credential;

        public UpdatePrabhariCommandHandler(IStatePrabhariRepository repo, CredentialManagerFunc credential)
        {
            _repo = repo;
            _credential = credential;
        }
        public async  Task<int> Handle(UpdatePrabhariCommand request, CancellationToken cancellationToken)
        {
            var  req = request.Dto;
            var data =  await _repo.GetByIdAsync(req.Id);
            if(data == null)
            {
                throw new NotFoundException("State Prabhari Not Found");
            }
            _credential.UpdateCredentialAsync(request.UserId, request.Dto.ContactNumber);

            data.Update(req.PrabhariName,req.PrabhariEmail,req.Gender,req.ContactNumber,req.CategoryId,req.CastId,req.Education,req.Profession,req.CurrentAddress);

            return await _repo.UpdateAsync(data);


        }
    }
}
