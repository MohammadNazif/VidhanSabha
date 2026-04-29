using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Interfaces;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Commands
{
    public class UpdateStateMembersCommandHandler : IRequestHandler<UpdateStateMembersCommand, int>
    {
        private readonly IImageService _imageService;
        private readonly IStateMembersRepository _repo;
        public UpdateStateMembersCommandHandler(IStateMembersRepository repo, IImageService imageService)
        {
            _imageService = imageService;
            _repo = repo;
        }
        public async Task<int> Handle(UpdateStateMembersCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var members = await _repo.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"State Member with Id {dto.Id} not found.");

            string? newImagePath = null;
            if (dto.Profile != null)
            {
                //if (!_imageService.IsValidImage(request.ProfileImage))
                //    throw new ValidationException("Invalid image. Only JPG/PNG/WEBP under 5MB allowed.");

                newImagePath = await _imageService.SaveImageAsync(
                    dto.Profile,
                    subFolder: "profiles/StateMember"
                );

                // Delete old image only after new one saved successfully
                await _imageService.DeleteImageAsync(members.Profile);
            }
            if (members == null)
            {
                throw new NotFoundException("State Members Not Found");
            }

            members.Update
                (
                dto.DesignationId, dto.DesignationTypeId, dto.Name, dto.Email, dto.Mobile, dto.CategoryId,
                dto.CastId, newImagePath, dto.Education, dto.DOB, dto.Address, dto.Proffesion
                );

            return _repo.Update(members);


        }

    }
}
