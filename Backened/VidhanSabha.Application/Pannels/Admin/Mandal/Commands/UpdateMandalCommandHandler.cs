using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Commands
{
    public class UpdateMandalCommandHandler : IRequestHandler<UpdateMandalCommand, MandalResponseDto>
    {
        private IMandalRepository _repo;

        public UpdateMandalCommandHandler(IMandalRepository repo)
        {
            _repo = repo;
        }
        public async Task<MandalResponseDto> Handle(UpdateMandalCommand request, CancellationToken cancellationToken)
        {
           var mandal =  await _repo.GetByIdAsync(request.Id);
            if (mandal == null)
            {
                throw new NotFoundException("Mandal not found");
            }

            mandal.Update(request.Name);
            await _repo.UpdateAsync(mandal);

            return new MandalResponseDto
            {
                Id = mandal.Id,
                VidhanId = mandal.VidhanId,
                Name = mandal.Name,
                Status = mandal.Status
            };
        }
    }
}
