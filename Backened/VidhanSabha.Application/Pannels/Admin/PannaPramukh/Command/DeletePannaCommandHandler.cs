using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command
{
    public class DeletePannaCommandHandler : IRequestHandler<DeletePannaCommand, int>
    {
        private IPannaPramukhRepository _repo;

        public DeletePannaCommandHandler(IPannaPramukhRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeletePannaCommand request,CancellationToken cancellationToken)
         {
            var panna = await _repo.GetByIdAsync(request.Id);

            if(panna == null)
            {
                throw new NotFoundException("Panna Pranukh Not Found");
            }

            panna.Delete();

            return  _repo.Update(panna);
        }

       
    }
}
