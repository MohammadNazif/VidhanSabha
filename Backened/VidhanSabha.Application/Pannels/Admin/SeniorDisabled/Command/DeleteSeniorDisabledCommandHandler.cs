using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Command
{
    public class DeleteSeniorDisabledCommandHandler : IRequestHandler<DeleteSeniorDisabledCommand, int> 
    {
        private ISeniorDisabledRepository _repo;

        public DeleteSeniorDisabledCommandHandler(ISeniorDisabledRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteSeniorDisabledCommand request, CancellationToken cancellationtoken)
        {
            var seniordisabled = await _repo.GetByIdAsync(request.Id);

            if (seniordisabled == null)
            {
                throw new NotFoundException("Senior/Disabled Not Found");
            }
            seniordisabled.Delete();
            return _repo.Update(seniordisabled);
        }
    }
}
