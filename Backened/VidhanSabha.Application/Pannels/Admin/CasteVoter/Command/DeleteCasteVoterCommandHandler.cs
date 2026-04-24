using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Command;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.CasteVoter.Command
{
    public class DeleteCasteVoterCommandHandler : IRequestHandler<DeleteCasteVoterCommand, int>
    {
        private ICasteVoterRepository _repo;

        public DeleteCasteVoterCommandHandler(ICasteVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteCasteVoterCommand request, CancellationToken cancellationToken)
        {
            var casteVoter = await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (casteVoter == null)
                throw new NotFoundException("CasteVoter not found");
            casteVoter.Delete();

            return await _repo.UpdateRangeAsync(
                new List<Tbl_CasteVoter> { casteVoter },
                cancellationToken
            );
        }
    }
}
