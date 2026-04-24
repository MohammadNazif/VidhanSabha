using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces; // Add this using directive at the top
using VidhanSabha.Domain.Entities.Admin; // Add this using directive at the top

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Commands.Create
{
    public class CreateMandalCommandHandler
         : IRequestHandler<CreateMandalCommand, MandalResponseDto>
    {
        private readonly IMandalRepository _repo;

        public CreateMandalCommandHandler(IMandalRepository repo)
        {
            _repo = repo;
        }

        public async Task<MandalResponseDto> Handle(
            CreateMandalCommand command, CancellationToken ct)
        {

            int? VidhanId =  await _repo.GetVidhansabhaIdByuserIdAsync(command.userId);
            if(VidhanId == 0)
            {
                throw new NotFoundException("VidhanSabha not found for the given user");
            }
            // Check duplicate name under same VidhanId
            var exists = await _repo.ExistsByNameAsync(VidhanId, command.Name);
            if (exists)
                throw new ValidationException(new Dictionary<string, string[]>
                {
                    { "Name", new[] { "Mandal name already exists for this VidhanId." } }
                });

            // Domain entity — validates + creates
            var mandal = Tbl_Mandal.Create(
                VidhanId,
                command.Name);

            await _repo.AddAsync(mandal);

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
