using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Command
{
    internal class UpdateMandalSamitiMemberCommandHandler : IRequestHandler<UpdateMandalSamitiMemberCommand, int>
    {
        private IMandalSamiti _repo;

        public UpdateMandalSamitiMemberCommandHandler(IMandalSamiti repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdateMandalSamitiMemberCommand req, CancellationToken cancellationToken)
        {
            var data = await _repo.GetMandalSamitiMemberByIdAsync(req.Dto.Id,cancellationToken);
             var request = req.Dto;
            data.Update(request.Name, request.Age, request.Contact, request.Occupation, request.DesignationId, request.CategoryId, request.CasteId);

             _repo.UpdateMandalSamitiMember(data);

            return await _repo.SaveChangesAsync(cancellationToken);
        }
    }
}
