using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Interface;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Command
{
    internal class DeleteVidhanSabhaCommandHandler : IRequestHandler<DeleteVidhanSabhaCommand, int>
    {
        private IVidhanSabhaRepository _repo;

        public DeleteVidhanSabhaCommandHandler(IVidhanSabhaRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(DeleteVidhanSabhaCommand request, CancellationToken cancellationToken)
        {
             var data = await _repo.GetVidhanSabhaByIdAsync(request.Id);
            if (data == null)
            {
                throw new NotFoundException("VidhanSabha Not Found");
            }

            data.Delete();
            return  await _repo.UpdateVidhanSabhaNameNumberAsync(data);
          

        }
    }
}
