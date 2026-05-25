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
    internal class UpdateVidhanSabhaNameNumberQueryHandler : IRequestHandler<UpdateVidhanSabhaNameNumberQuery, int>
    {
        private IVidhanSabhaRepository _repo;

        public UpdateVidhanSabhaNameNumberQueryHandler(IVidhanSabhaRepository
             repo)
        {
            _repo = repo; 
        }
        public async Task<int> Handle(UpdateVidhanSabhaNameNumberQuery request, CancellationToken cancellationToken)
        {

               var data = await _repo.GetVidhanSabhaByIdAsync(request.Dto.Id);
            if(data == null)
            {
                throw new NotFoundException("VidhanSabha Not Found");
            }
            data.Update(request.Dto.VidhanSabhaName, request.Dto.VidhanSabhaNumber);

               return await _repo.UpdateVidhanSabhaNameNumberAsync(data);
        }
    }
}
